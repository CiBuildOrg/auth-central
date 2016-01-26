using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    internal class MembershipRebootSetup : MembershipRebootConfiguration<HierarchicalUserAccount>
    {
        public SecuritySettings SecuritySettings { get; set; }
    }

    internal sealed class AuthCentralAppInfo : RelativePathApplicationInformation
    {
        public AuthCentralAppInfo(
            string baseUrl,
            string appName,
            string emailSig,
            string relativeLoginUrl,
            string relativeConfirmChangeEmailUrl,
            string relativeCancelNewAccountUrl,
            string relativeConfirmPasswordResetUrl)
        {
            SetBaseUrl(baseUrl);
            ApplicationName = appName;
            EmailSignature = emailSig;
            RelativeLoginUrl = relativeLoginUrl;
            RelativeCancelVerificationUrl = relativeCancelNewAccountUrl;
            RelativeConfirmPasswordResetUrl = relativeConfirmPasswordResetUrl;
            RelativeConfirmChangeEmailUrl = relativeConfirmChangeEmailUrl;
        }
    }
}
