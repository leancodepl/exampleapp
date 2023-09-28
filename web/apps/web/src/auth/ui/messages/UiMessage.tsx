/* eslint-disable @typescript-eslint/no-explicit-any */
import { createContext, ReactNode, useContext } from "react";
import {
    ErrorValidation,
    ErrorValidationRecovery,
    ErrorValidationVerification,
    InfoNodeLabel,
    InfoSelfServiceLogin,
    InfoSelfServiceRecovery,
    InfoSelfServiceRegistration,
    InfoSelfServiceSettings,
    InfoSelfServiceVerification,
} from "@leancodepl/kratos";
import { UiNodeInputAttributes, UiNodeTextAttributes, UiText } from "@ory/kratos-client";
import { FormattedMessage } from "react-intl";

type UiMessageProps = {
    text?: UiText;
    attributes?: UiNodeInputAttributes | UiNodeTextAttributes;
};

export function UiMessage({ text, attributes }: UiMessageProps) {
    const customUiMessage = useContext(customUiMessageContext);

    return <>{uiMessageRenderer({ attributes, customUiMessage, text })}</>;
}

export function uiMessageRenderer({
    text,
    attributes,
    customUiMessage,
}: UiMessageProps & { customUiMessage?: CustomUiMessage }): ReactNode {
    if (customUiMessage) {
        return customUiMessage({ attributes, text, uiMessage: uiMessageContent });
    }

    return uiMessageContent({ attributes, text });
}

// https://pkg.go.dev/github.com/ory/kratos/text#pkg-types
// https://github.com/ory/docs/blob/master/docs/kratos/concepts/messages.json
function uiMessageContent({ text: uiText, attributes }: UiMessageProps): ReactNode {
    if (!uiText) return null;

    const { id, text, context } = uiText;

    switch (id) {
        case InfoSelfServiceLogin.InfoSelfServiceLogin:
            return <FormattedMessage defaultMessage="Sign in" />;
        case InfoSelfServiceLogin.InfoSelfServiceLoginWith:
            return <FormattedMessage defaultMessage="Sign in with {provider}" values={context as any} />;
        case InfoSelfServiceLogin.InfoSelfServiceLoginReAuth:
            return <FormattedMessage defaultMessage="Please confirm this action by verifying that it is you." />;
        case InfoSelfServiceLogin.InfoSelfServiceLoginMFA:
            return <FormattedMessage defaultMessage="Please complete the second authentication challenge" />;
        case InfoSelfServiceLogin.InfoSelfServiceLoginVerify:
            return <FormattedMessage defaultMessage="Verify" />;
        case InfoSelfServiceLogin.InfoSelfServiceLoginTOTPLabel:
            return <FormattedMessage defaultMessage="Authentication code" />;
        case InfoSelfServiceLogin.InfoLoginLookupLabel:
            return <FormattedMessage defaultMessage="Backup recovery code" />;
        case InfoSelfServiceLogin.InfoSelfServiceLoginWebAuthn:
            return <FormattedMessage defaultMessage="Use security key" />;
        case InfoSelfServiceLogin.InfoLoginTOTP:
            return <FormattedMessage defaultMessage="Use Authenticator" />;
        case InfoSelfServiceLogin.InfoLoginLookup:
            return <FormattedMessage defaultMessage="Use backup recovery code" />;
        case InfoSelfServiceLogin.InfoSelfServiceLoginContinueWebAuthn:
            return <FormattedMessage defaultMessage="Continue with security key" />;
        case InfoSelfServiceLogin.InfoSelfServiceLoginWebAuthnPasswordless:
            return (
                <FormattedMessage defaultMessage="Prepare your WebAuthn device (e.g. security key, biometrics scanner, ...) and press continue." />
            );
        case InfoSelfServiceLogin.InfoSelfServiceLoginContinue:
            return <FormattedMessage defaultMessage="Continue" />;

        case InfoSelfServiceRegistration.InfoSelfServiceRegistration:
            return <FormattedMessage defaultMessage="Sign up" />;
        case InfoSelfServiceRegistration.InfoSelfServiceRegistrationWith:
            return <FormattedMessage defaultMessage="Sign up with {provider}" values={context as any} />;
        case InfoSelfServiceRegistration.InfoSelfServiceRegistrationContinue:
            return <FormattedMessage defaultMessage="Continue" />;
        case InfoSelfServiceRegistration.InfoSelfServiceRegistrationRegisterWebAuthn:
            return <FormattedMessage defaultMessage="Sign up with security key" />;

        case InfoSelfServiceSettings.InfoSelfServiceSettingsUpdateSuccess:
            return <FormattedMessage defaultMessage="Your changes have been saved!" />;
        case InfoSelfServiceSettings.InfoSelfServiceSettingsUpdateLinkOidc:
            return <FormattedMessage defaultMessage="Link {provider}" values={context as any} />;
        case InfoSelfServiceSettings.InfoSelfServiceSettingsUpdateUnlinkOidc:
            return <FormattedMessage defaultMessage="Unlink {provider}" values={context as any} />;
        case InfoSelfServiceSettings.InfoSelfServiceSettingsUpdateUnlinkTOTP:
            return <FormattedMessage defaultMessage="Unlink TOTP Authenticator App" />;
        case InfoSelfServiceSettings.InfoSelfServiceSettingsTOTPSecretLabel:
            return (
                <FormattedMessage defaultMessage="This is your authenticator app secret. Use it if you can not scan the QR code." />
            );
        case InfoSelfServiceRecovery.InfoSelfServiceRecoverySuccessful:
            return (
                <FormattedMessage defaultMessage="You successfully recovered your account. Please change your password or set up an alternative login method (e.g. social sign in) within the next 1.00 minutes." />
            );
        case InfoSelfServiceRecovery.InfoSelfServiceRecoveryEmailSent:
            return (
                <FormattedMessage defaultMessage="An email containing a recovery link has been sent to the email address you provided. If you have not received an email, check the spelling of the address and make sure to use the address you registered with." />
            );
        case InfoSelfServiceRecovery.InfoSelfServiceRecoveryEmailWithCodeSent:
            return (
                <FormattedMessage defaultMessage="An email containing a recovery code has been sent to the email address you provided. If you have not received an email, check the spelling of the address and make sure to use the address you registered with." />
            );

        case InfoNodeLabel.InfoNodeLabelInputPassword:
            return <FormattedMessage defaultMessage="Password" />;
        case InfoNodeLabel.InfoNodeLabelSave:
            return <FormattedMessage defaultMessage="Save" />;
        case InfoNodeLabel.InfoNodeLabelID:
            return <FormattedMessage defaultMessage="ID" />;
        case InfoNodeLabel.InfoNodeLabelSubmit:
            return <FormattedMessage defaultMessage="Submit" />;
        case InfoNodeLabel.InfoNodeLabelVerifyOTP:
            return <FormattedMessage defaultMessage="Verify code" />;
        case InfoNodeLabel.InfoNodeLabelEmail:
            return <FormattedMessage defaultMessage="E-mail" />;
        case InfoNodeLabel.InfoNodeLabelResendOTP:
            return <FormattedMessage defaultMessage="Resend code" />;
        case InfoNodeLabel.InfoNodeLabelContinue:
            return <FormattedMessage defaultMessage="Continue" />;
        case InfoNodeLabel.InfoNodeLabelRecoveryCode:
            return <FormattedMessage defaultMessage="Recovery code" />;
        case InfoNodeLabel.InfoNodeLabelVerificationCode:
            return <FormattedMessage defaultMessage="Verification code" />;

        case InfoSelfServiceVerification.InfoSelfServiceVerificationEmailSent:
            return (
                <FormattedMessage defaultMessage="An email containing a verification link has been sent to the email address you provided. If you have not received an email, check the spelling of the address and make sure to use the address you registered with." />
            );
        case InfoSelfServiceVerification.InfoSelfServiceVerificationSuccessful:
            return <FormattedMessage defaultMessage="You successfully verified your email address." />;
        case InfoSelfServiceVerification.InfoSelfServiceVerificationEmailWithCodeSent:
            return (
                <FormattedMessage defaultMessage="An email containing a verification code has been sent to the email address you provided. If you have not received an email, check the spelling of the address and make sure to use the address you registered with." />
            );
        case ErrorValidation.ErrorValidationRequired:
            return <FormattedMessage defaultMessage="Property {property} is missing." values={context as any} />;
        case ErrorValidation.ErrorValidationInvalidFormat:
            return <FormattedMessage defaultMessage="Does not match pattern '{pattern}'" values={context as any} />;
        case ErrorValidation.ErrorValidationPasswordPolicyViolation:
            return (
                <FormattedMessage
                    defaultMessage="The password can not be used because {reason}."
                    values={context as any}
                />
            );
        case ErrorValidation.ErrorValidationInvalidCredentials:
            return (
                <FormattedMessage defaultMessage="The provided credentials are invalid, check for spelling mistakes in your password or username, email address, or phone number." />
            );
        case ErrorValidation.ErrorValidationDuplicateCredentials:
            return (
                <FormattedMessage defaultMessage="An account with the same identifier (email, phone, username, ...) exists already." />
            );
        case ErrorValidation.ErrorValidationTOTPVerifierWrong:
            return <FormattedMessage defaultMessage="The provided authentication code is invalid, please try again." />;
        case ErrorValidation.ErrorValidationIdentifierMissing:
            return (
                <FormattedMessage defaultMessage="Could not find any login identifiers. Did you forget to set them? This could also be caused by a server misconfiguration." />
            );
        case ErrorValidation.ErrorValidationAddressNotVerified:
            return (
                <FormattedMessage defaultMessage="Account not active yet. Did you forget to verify your email address?" />
            );
        case ErrorValidation.ErrorValidationNoTOTPDevice:
            return <FormattedMessage defaultMessage="You have no TOTP device set up." />;
        case ErrorValidation.ErrorValidationLookupAlreadyUsed:
            return <FormattedMessage defaultMessage="This backup recovery code has already been used." />;
        case ErrorValidation.ErrorValidationNoWebAuthnDevice:
            return <FormattedMessage defaultMessage="You have no WebAuthn device set up." />;
        case ErrorValidation.ErrorValidationNoLookup:
            return <FormattedMessage defaultMessage="You have no backup recovery codes set up." />;
        case ErrorValidation.ErrorValidationSuchNoWebAuthnUser:
            return <FormattedMessage defaultMessage="This account does not exist or has no security key set up." />;
        case ErrorValidation.ErrorValidationLookupInvalid:
            return <FormattedMessage defaultMessage="The backup recovery code is not valid." />;

        case ErrorValidationRecovery.ErrorValidationRecoveryRetrySuccess:
            return (
                <FormattedMessage defaultMessage="The request was already completed successfully and can not be retried." />
            );
        case ErrorValidationRecovery.ErrorValidationRecoveryStateFailure:
            return <FormattedMessage defaultMessage="The recovery flow reached a failure state and must be retried." />;
        case ErrorValidationRecovery.ErrorValidationRecoveryMissingRecoveryToken:
            return <FormattedMessage defaultMessage="The recovery token is missing. Please retry the flow." />;
        case ErrorValidationRecovery.ErrorValidationRecoveryTokenInvalidOrAlreadyUsed:
            return (
                <FormattedMessage defaultMessage="The recovery token is invalid or has already been used. Please retry the flow." />
            );
        case ErrorValidationRecovery.ErrorValidationRecoveryFlowExpired:
            return <FormattedMessage defaultMessage="The recovery flow expired, please try again." />;
        case ErrorValidationRecovery.ErrorValidationRecoveryCodeInvalidOrAlreadyUsed:
            return (
                <FormattedMessage defaultMessage="The recovery code is invalid or has already been used. Please retry the flow." />
            );

        case ErrorValidationVerification.ErrorValidationVerificationTokenInvalidOrAlreadyUsed:
            return (
                <FormattedMessage defaultMessage="The verification token is invalid or has already been used. Please retry the flow." />
            );
        case ErrorValidationVerification.ErrorValidationVerificationRetrySuccess:
            return (
                <FormattedMessage defaultMessage="The request was already completed successfully and can not be retried." />
            );
        case ErrorValidationVerification.ErrorValidationVerificationStateFailure:
            return (
                <FormattedMessage defaultMessage="The verification flow reached a failure state and must be retried." />
            );
        case ErrorValidationVerification.ErrorValidationVerificationMissingVerificationToken:
            return <FormattedMessage defaultMessage="The verification token is missing. Please retry the flow." />;
        case ErrorValidationVerification.ErrorValidationVerificationFlowExpired:
            return <FormattedMessage defaultMessage="The verification flow expired, please try again." />;
        case ErrorValidationVerification.ErrorValidationVerificationCodeInvalidOrAlreadyUsed:
            return (
                <FormattedMessage defaultMessage="The verification code is invalid or has already been used. Please try again." />
            );
    }

    // eslint-disable-next-line react/jsx-no-useless-fragment
    return <>{text}</>;
}

export type CustomUiMessageParams = {
    text?: UiText;
    attributes?: UiNodeInputAttributes | UiNodeTextAttributes;
    uiMessage: typeof uiMessageContent;
};

export type CustomUiMessage = (params: CustomUiMessageParams) => ReactNode;

const customUiMessageContext = createContext<CustomUiMessage | undefined>(undefined);

type CustomGetMessageProviderProps = {
    uiMessage?: CustomUiMessage;
    children?: ReactNode;
};

export function CustomGetMessageProvider({ uiMessage, children }: CustomGetMessageProviderProps) {
    // eslint-disable-next-line react/jsx-no-useless-fragment
    if (!uiMessage) return <>{children}</>;

    return <customUiMessageContext.Provider value={uiMessage}>{children}</customUiMessageContext.Provider>;
}

export function useCustomUiMessageContext() {
    return useContext(customUiMessageContext);
}
