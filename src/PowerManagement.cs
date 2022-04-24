using System.Runtime.InteropServices;
using System.Text;

namespace PowerSettings
{
    public static class PowerManagement
    {
        private static readonly Guid NO_SUBGROUP_GUID = new("fea3413e-7e05-4911-9a71-700331f1c294");
        private static readonly Guid GUID_DISK_SUBGROUP = new("0012ee47-9041-4b5d-9b77-535fba8b1442");
        private static readonly Guid GUID_SYSTEM_BUTTON_SUBGROUP = new("4f971e89-eebd-4455-a8de-9e59040e7347");
        private static readonly Guid GUID_PROCESSOR_SETTINGS_SUBGROUP = new("54533251-82be-4824-96c1-47b60b740d00");
        private static readonly Guid GUID_VIDEO_SUBGROUP = new("7516b95f-f776-4464-8c53-06167f40cc99");
        private static readonly Guid GUID_BATTERY_SUBGROUP = new("e73a048d-bf27-4f12-9731-8b2076e8891f");
        private static readonly Guid GUID_SLEEP_SUBGROUP = new("238C9FA8-0AAD-41ED-83F4-97BE242C8F20");
        private static readonly Guid GUID_PCIEXPRESS_SETTINGS_SUBGROUP = new("501a4d13-42af-4429-9fd1-a8218c268e20");
        private static readonly Guid GUID_LOCK_CONSOLE_ON_WAKE = new("0e796bdb-100d-47d6-a2d5-f7d2daa51f51");
        private static readonly Guid GUID_DISK_POWERDOWN_TIMEOUT = new("6738e2c4-e8a5-4a42-b16a-e040e769756e");
        private static readonly Guid GUID_STANDBY_TIMEOUT = new("29f6c1db-86da-48c5-9fdb-f2b67b1f44da");
        private static readonly Guid GUID_HIBERNATE_TIMEOUT = new("9d7815a6-7ee4-497e-8888-515a05f02364");
        private static readonly Guid GUID_LIDCLOSE_ACTION = new("5ca83367-6e45-459f-a27b-476b1d01c936");
        private static readonly Guid GUID_POWERBUTTON_ACTION = new("7648efa3-dd9c-4e3e-b566-50f929386280");
        private static readonly Guid GUID_SLEEPBUTTON_ACTION = new("96996bc0-ad50-47ec-923b-6f41874dd9eb");
        private static readonly Guid GUID_VIDEO_POWERDOWN_TIMEOUT = new("3c0bc021-c8a8-4e07-a973-6b14cbcb2b7e");
        private static readonly Guid GUID_DEVICE_POWER_POLICY_VIDEO_BRIGHTNESS = new("aded5e82-b909-4619-9949-f5d71dac0bcb");

        public static Guid GetActiveSchemeGuid()
        {
            IntPtr zero = IntPtr.Zero;
            SystemErrorCodes activeScheme = PowerGetActiveScheme(IntPtr.Zero, ref zero);
            if (activeScheme != SystemErrorCodes.ERROR_SUCCESS)
                throw new Exception("PowerGetActiveScheme; Error:" + activeScheme);
            Guid structure = Marshal.PtrToStructure<Guid>(zero);
            Marshal.FreeHGlobal(zero);
            return structure;
        }

        public static string GetActiveSchemeName() => GetSchemeName(GetActiveSchemeGuid());

        public static void SetActiveScheme(Guid guid)
        {
            IntPtr num = Marshal.AllocHGlobal(Marshal.SizeOf(guid));
            Marshal.StructureToPtr(guid, num, true);
            SystemErrorCodes systemErrorCodes = PowerSetActiveScheme(IntPtr.Zero, num);
            if (systemErrorCodes != SystemErrorCodes.ERROR_SUCCESS)
                throw new Exception("PowerSetActiveScheme; Error:" + systemErrorCodes);
            Marshal.FreeHGlobal(num);
        }

        public static void SetActiveScheme(string name)
        {
            Guid schemeGuid = GetSchemeGuid(name);
            if (schemeGuid == Guid.Empty)
                throw new Exception("Powerscheme [" + name + "] does not exist.");
            SetActiveScheme(schemeGuid);
        }

        public static string GetSchemeName(Guid guid)
        {
            IntPtr num1 = Marshal.AllocHGlobal(Marshal.SizeOf(guid));
            Marshal.StructureToPtr(guid, num1, true);
            int BufferSize = 0;
            SystemErrorCodes systemErrorCodes1 = PowerReadFriendlyName(IntPtr.Zero, num1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, ref BufferSize);
            if (systemErrorCodes1 != SystemErrorCodes.ERROR_SUCCESS)
                throw new Exception("PowerReadFriendlyName; Error:" + systemErrorCodes1);
            IntPtr num2 = Marshal.AllocHGlobal(BufferSize);
            SystemErrorCodes systemErrorCodes2 = PowerReadFriendlyName(IntPtr.Zero, num1, IntPtr.Zero, IntPtr.Zero, num2, ref BufferSize);
            if (systemErrorCodes2 != SystemErrorCodes.ERROR_SUCCESS)
                throw new Exception("PowerReadFriendlyName; Error:" + systemErrorCodes2);
            string stringUni = Marshal.PtrToStringUni(num2)!;
            Marshal.FreeHGlobal(num2);
            Marshal.FreeHGlobal(num1);
            return stringUni;
        }

        public static void SetSchemeName(Guid guid, string name)
        {
            IntPtr num1 = Marshal.AllocHGlobal(Marshal.SizeOf(guid));
            Marshal.StructureToPtr(guid, num1, true);
            int byteCount = Encoding.Unicode.GetByteCount(name + "\0");
            IntPtr hglobalUni = Marshal.StringToHGlobalUni(name);
            _ = (int)PowerWriteFriendlyName(IntPtr.Zero, num1, IntPtr.Zero, IntPtr.Zero, hglobalUni, byteCount);
            Marshal.FreeHGlobal(num1);
            Marshal.FreeHGlobal(hglobalUni);
        }

        public static string GetSchemeDescription(Guid guid)
        {
            IntPtr num1 = Marshal.AllocHGlobal(Marshal.SizeOf(guid));
            Marshal.StructureToPtr(guid, num1, true);
            int BufferSize = 0;
            SystemErrorCodes systemErrorCodes1 = PowerReadDescription(IntPtr.Zero, num1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, ref BufferSize);
            if (systemErrorCodes1 != SystemErrorCodes.ERROR_SUCCESS)
                throw new Exception("PowerReadDescription; Error:" + systemErrorCodes1);
            IntPtr num2 = Marshal.AllocHGlobal(BufferSize);
            SystemErrorCodes systemErrorCodes2 = PowerReadDescription(IntPtr.Zero, num1, IntPtr.Zero, IntPtr.Zero, num2, ref BufferSize);
            if (systemErrorCodes2 != SystemErrorCodes.ERROR_SUCCESS)
                throw new Exception("PowerReadDescription; Error:" + systemErrorCodes2);
            string stringUni = Marshal.PtrToStringUni(num2)!;
            Marshal.FreeHGlobal(num2);
            Marshal.FreeHGlobal(num1);
            return stringUni;
        }

        public static void SetSchemeDescription(Guid guid, string description)
        {
            IntPtr num1 = Marshal.AllocHGlobal(Marshal.SizeOf(guid));
            Marshal.StructureToPtr(guid, num1, true);
            int byteCount = Encoding.Unicode.GetByteCount(description + "\0");
            IntPtr hglobalUni = Marshal.StringToHGlobalUni(description);
            _ = (int)PowerWriteDescription(IntPtr.Zero, num1, IntPtr.Zero, IntPtr.Zero, hglobalUni, byteCount);
            Marshal.FreeHGlobal(num1);
            Marshal.FreeHGlobal(hglobalUni);
        }

        public static int GetSchemeACValue(Guid schemeGuid)
        {
            IntPtr num1 = Marshal.AllocHGlobal(Marshal.SizeOf(schemeGuid));
            Marshal.StructureToPtr(schemeGuid, num1, true);
            IntPtr num2 = Marshal.AllocHGlobal(Marshal.SizeOf(schemeGuid));
            Marshal.StructureToPtr(schemeGuid, num2, true);
            IntPtr num3 = Marshal.AllocHGlobal(Marshal.SizeOf(schemeGuid));
            Marshal.StructureToPtr(schemeGuid, num3, true);
            int AcValueIndex = 0;
            _ = (int)PowerReadACValueIndex(IntPtr.Zero, num1, num2, num3, ref AcValueIndex);
            Marshal.FreeHGlobal(num1);
            Marshal.FreeHGlobal(num2);
            Marshal.FreeHGlobal(num3);
            return AcValueIndex;
        }

        public static int GetSchemeDCValue(Guid schemeGuid)
        {
            IntPtr num1 = Marshal.AllocHGlobal(Marshal.SizeOf(schemeGuid));
            Marshal.StructureToPtr(schemeGuid, num1, true);
            IntPtr num2 = Marshal.AllocHGlobal(Marshal.SizeOf(schemeGuid));
            Marshal.StructureToPtr(schemeGuid, num2, true);
            IntPtr num3 = Marshal.AllocHGlobal(Marshal.SizeOf(schemeGuid));
            Marshal.StructureToPtr(schemeGuid, num3, true);
            int DcValueIndex = 0;
            _ = (int)PowerReadDCValueIndex(IntPtr.Zero, num1, num2, num3, ref DcValueIndex);
            Marshal.FreeHGlobal(num1);
            Marshal.FreeHGlobal(num2);
            Marshal.FreeHGlobal(num3);
            return DcValueIndex;
        }

        public static void SetSchemeACValue(
          Guid schemeGuid,
          int value)
        {
            IntPtr num1 = Marshal.AllocHGlobal(Marshal.SizeOf(schemeGuid));
            Marshal.StructureToPtr(schemeGuid, num1, true);
            IntPtr num2 = Marshal.AllocHGlobal(Marshal.SizeOf(schemeGuid));
            Marshal.StructureToPtr(schemeGuid, num2, true);
            IntPtr num3 = Marshal.AllocHGlobal(Marshal.SizeOf(schemeGuid));
            Marshal.StructureToPtr(schemeGuid, num3, true);
            _ = (int)PowerWriteACValueIndex(IntPtr.Zero, num1, num2, num3, value);
            Marshal.FreeHGlobal(num1);
            Marshal.FreeHGlobal(num2);
            Marshal.FreeHGlobal(num3);
        }

        public static void SetSchemeDCValue(
          Guid schemeGuid,
          int value)
        {
            IntPtr num1 = Marshal.AllocHGlobal(Marshal.SizeOf(schemeGuid));
            Marshal.StructureToPtr(schemeGuid, num1, true);
            IntPtr num2 = Marshal.AllocHGlobal(Marshal.SizeOf(schemeGuid));
            IntPtr num3 = num2;
            Marshal.StructureToPtr(num3, num3, true);
            IntPtr num4 = Marshal.AllocHGlobal(Marshal.SizeOf(schemeGuid));
            IntPtr num5 = num4;
            Marshal.StructureToPtr(num5, num5, true);
            _ = (int)PowerWriteDCValueIndex(IntPtr.Zero, num1, num2, num4, value);
            Marshal.FreeHGlobal(num1);
            Marshal.FreeHGlobal(num2);
            Marshal.FreeHGlobal(num4);
        }

        public static Guid GetSchemeGuid(string name)
        {
            SystemErrorCodes systemErrorCodes = SystemErrorCodes.ERROR_SUCCESS;
            uint Index = 0;
            int BufferSize = Marshal.SizeOf(typeof(Guid));
            IntPtr num = Marshal.AllocHGlobal(BufferSize);
            Guid guid = Guid.Empty;
            while (systemErrorCodes == SystemErrorCodes.ERROR_SUCCESS)
            {
                systemErrorCodes = PowerEnumerate(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, PowerDataAccessor.ACCESS_SCHEME, Index, num, ref BufferSize);
                if (systemErrorCodes != SystemErrorCodes.ERROR_NO_MORE_ITEMS)
                {
                    if (systemErrorCodes != SystemErrorCodes.ERROR_SUCCESS)
                        throw new Exception("PowerEnumerate; Error:" + systemErrorCodes);
                    guid = Marshal.PtrToStructure<Guid>(num);
                    if (!(GetSchemeName(guid) == name))
                        ++Index;
                    else
                        break;
                }
                else
                    break;
            }
            Marshal.FreeHGlobal(num);
            return guid;
        }

        public static Guid DuplicateScheme(Guid guidSource)
        {
            IntPtr num = Marshal.AllocHGlobal(Marshal.SizeOf(guidSource));
            Marshal.StructureToPtr(guidSource, num, true);
            IntPtr zero = IntPtr.Zero;
            SystemErrorCodes systemErrorCodes = PowerDuplicateScheme(IntPtr.Zero, num, ref zero);
            if (systemErrorCodes != SystemErrorCodes.ERROR_SUCCESS)
                throw new Exception("PowerDuplicateScheme; Error:" + systemErrorCodes);
            Guid structure = Marshal.PtrToStructure<Guid>(zero);
            Marshal.FreeHGlobal(num);
            Marshal.FreeHGlobal(zero);
            return structure;
        }





        [DllImport("powrprof.dll")]
        private static extern SystemErrorCodes PowerGetActiveScheme(
          IntPtr RootPowerKey,
          ref IntPtr ActivePolicyGuid);

        [DllImport("powrprof.dll")]
        private static extern SystemErrorCodes PowerSetActiveScheme(
          IntPtr RootPowerKey,
          IntPtr SchemeGuid);

        [DllImport("powrprof.dll")]
        private static extern SystemErrorCodes PowerDuplicateScheme(
          IntPtr RootPowerKey,
          IntPtr SrcSchemeGuid,
          ref IntPtr DstSchemeGuid);

        [DllImport("powrprof.dll")]
        private static extern SystemErrorCodes PowerEnumerate(
          IntPtr UserRootPowerKey,
          IntPtr SchemeGuid,
          IntPtr SubGroupOfPowerSettingsGuid,
          PowerDataAccessor AccessFlags,
          uint Index,
          IntPtr Buffer,
          ref int BufferSize);

        [DllImport("powrprof.dll")]
        private static extern SystemErrorCodes PowerReadFriendlyName(
          IntPtr RootPowerKey,
          IntPtr SchemeGuid,
          IntPtr SubGroupOfPowerSettingGuid,
          IntPtr PowerSettingGuid,
          IntPtr Buffer,
          ref int BufferSize);

        [DllImport("powrprof.dll")]
        private static extern SystemErrorCodes PowerWriteFriendlyName(
          IntPtr RootPowerKey,
          IntPtr SchemeGuid,
          IntPtr SubGroupOfPowerSettingGuid,
          IntPtr PowerSettingGuid,
          IntPtr Buffer,
          int BufferSize);

        [DllImport("powrprof.dll")]
        private static extern SystemErrorCodes PowerReadDescription(
          IntPtr RootPowerKey,
          IntPtr SchemeGuid,
          IntPtr SubGroupOfPowerSettingGuid,
          IntPtr PowerSettingGuid,
          IntPtr Buffer,
          ref int BufferSize);

        [DllImport("powrprof.dll")]
        private static extern SystemErrorCodes PowerWriteDescription(
          IntPtr RootPowerKey,
          IntPtr SchemeGuid,
          IntPtr SubGroupOfPowerSettingGuid,
          IntPtr PowerSettingGuid,
          IntPtr Buffer,
          int BufferSize);

        [DllImport("powrprof.dll")]
        private static extern SystemErrorCodes PowerReadACValueIndex(
          IntPtr RootPowerKey,
          IntPtr SchemeGuid,
          IntPtr SubGroupOfPowerSettingGuid,
          IntPtr PowerSettingGuid,
          ref int AcValueIndex);

        [DllImport("powrprof.dll")]
        private static extern SystemErrorCodes PowerReadDCValueIndex(
          IntPtr RootPowerKey,
          IntPtr SchemeGuid,
          IntPtr SubGroupOfPowerSettingGuid,
          IntPtr PowerSettingGuid,
          ref int DcValueIndex);

        [DllImport("powrprof.dll")]
        private static extern SystemErrorCodes PowerWriteACValueIndex(
          IntPtr RootPowerKey,
          IntPtr SchemeGuid,
          IntPtr SubGroupOfPowerSettingGuid,
          IntPtr PowerSettingGuid,
          int AcValueIndex);

        [DllImport("powrprof.dll")]
        private static extern SystemErrorCodes PowerWriteDCValueIndex(
          IntPtr RootPowerKey,
          IntPtr SchemeGuid,
          IntPtr SubGroupOfPowerSettingGuid,
          IntPtr PowerSettingGuid,
          int DcValueIndex);

        private enum PowerDataAccessor : uint
        {
            ACCESS_AC_POWER_SETTING_INDEX = 0,
            ACCESS_DC_POWER_SETTING_INDEX = 1,
            ACCESS_SCHEME = 16, // 0x00000010
            ACCESS_SUBGROUP = 17, // 0x00000011
            ACCESS_INDIVIDUAL_SETTING = 18, // 0x00000012
            ACCESS_ACTIVE_SCHEME = 19, // 0x00000013
            ACCESS_CREATE_SCHEME = 20, // 0x00000014
        }

        private enum SystemErrorCodes : uint
        {
            ERROR_SUCCESS = 0,
            ERROR_NO_MORE_ITEMS = 259, // 0x00000103
        }
    }
}
