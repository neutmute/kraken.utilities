using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32;
using Common.Logging;

namespace Kraken.Core
{
    public static class UacHelper
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger("UacHelper");
        private const string uacRegistryKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
        private const string uacRegistryValue = "EnableLUA";

        private static uint STANDARD_RIGHTS_READ = 0x00020000;
        private static uint TOKEN_QUERY = 0x0008;
        private static uint TOKEN_READ = (STANDARD_RIGHTS_READ | TOKEN_QUERY);

        #endregion

        #region External

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, IntPtr TokenInformation, uint TokenInformationLength, out uint ReturnLength);

        public enum TOKEN_INFORMATION_CLASS
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId,
            TokenGroupsAndPrivileges,
            TokenSessionReference,
            TokenSandBoxInert,
            TokenAuditPolicy,
            TokenOrigin,
            TokenElevationType,
            TokenLinkedToken,
            TokenElevation,
            TokenHasRestrictions,
            TokenAccessInformation,
            TokenVirtualizationAllowed,
            TokenVirtualizationEnabled,
            TokenIntegrityLevel,
            TokenUIAccess,
            TokenMandatoryPolicy,
            TokenLogonSid,
            MaxTokenInfoClass
        }

        public enum TOKEN_ELEVATION_TYPE
        {
            TokenElevationTypeDefault = 1,
            TokenElevationTypeFull,
            TokenElevationTypeLimited
        }

        #endregion

        #region Methods
        public static string GetMetadataForLog()
        {
            return string.Format(
                "IsUacEnabled={0}, IsUserInAdminRole={1}, IsProcessElevated={2}"
                , IsUacEnabled
                , IsCurrentUserAdministrator
                , IsProcessElevated);
        }

        public static bool IsUacEnabled
        {
            get
            {
                RegistryKey uacKey = Registry.LocalMachine.OpenSubKey(uacRegistryKey, false);
                bool result = uacKey.GetValue(uacRegistryValue).Equals(1);
                return result;
            }
        }

        public static bool IsProcessElevated
        {
            get
            {
                if (IsUacEnabled)
                {
                    IntPtr tokenHandle;
                    if (!OpenProcessToken(Process.GetCurrentProcess().Handle, TOKEN_READ, out tokenHandle))
                    {
                        throw new ApplicationException("Could not get process token.  Win32 Error Code: " + Marshal.GetLastWin32Error());
                    }

                    TOKEN_ELEVATION_TYPE elevationResult = TOKEN_ELEVATION_TYPE.TokenElevationTypeDefault;

                    int elevationResultSize = Marshal.SizeOf((int)elevationResult);
                    uint returnedSize = 0;
                    IntPtr elevationTypePtr = Marshal.AllocHGlobal(elevationResultSize);

                    bool success = GetTokenInformation(tokenHandle, TOKEN_INFORMATION_CLASS.TokenElevationType, elevationTypePtr, (uint)elevationResultSize, out returnedSize);
                    if (success)
                    {
                        elevationResult = (TOKEN_ELEVATION_TYPE)Marshal.ReadInt32(elevationTypePtr);
                        bool isProcessAdmin = elevationResult == TOKEN_ELEVATION_TYPE.TokenElevationTypeFull;
                        return isProcessAdmin;
                    }
                    else
                    {
                        throw new ApplicationException("Unable to determine the current elevation.");
                    }
                }
                else
                {
                    return IsCurrentUserAdministrator;
                }
            }
        }

        public static bool IsCurrentUserAdministrator
        {
            get
            {
                var user = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(user);
                return IsUserAdministrator(principal);
            }
        }

        //public static bool IsUserAdministrator(string username)
        //{
        //    bool isAdmin;
        //    using (var identity = new WindowsIdentity(username))
        //    {
        //        var principal = new WindowsPrincipal(identity);
        //        try
        //        {
        //            isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
        //        }
        //        catch (UnauthorizedAccessException ex)
        //        {
        //            Log.WarnException("IsUserAdministrator", ex);
        //            isAdmin = false;
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.WarnException("IsUserAdministrator", ex);
        //            isAdmin = false;
        //        }
        //    }
        //    return isAdmin;
        //}


        private static bool IsUserAdministrator(WindowsPrincipal principal)
        {
            bool isAdmin;
            try
            {
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);   
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Warn("IsUserAdministrator", ex);
                isAdmin = false;
            }
            catch (Exception ex)
            {
                Log.Warn("IsUserAdministrator", ex);
                isAdmin = false;
            }
            return isAdmin;
        }
        #endregion
    }
}
