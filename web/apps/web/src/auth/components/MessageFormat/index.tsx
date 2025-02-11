import { ReactNode } from "react"
import { FormattedMessage } from "react-intl"
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
  MessageFormatComponentProps,
} from "@leancodepl/kratos"
import { ThemeComponent } from "../../../components/ThemeComponent"
import IconPasskeyDark from "./icon_passkey_dark.svg?react"
import IconPasskeyLight from "./icon_passkey_light.svg?react"
import { ProviderMessage } from "./ProviderMessage"

export function MessageFormat({ id, text, context }: MessageFormatComponentProps): ReactNode {
  switch (id) {
    case InfoSelfServiceLogin.InfoSelfServiceLogin:
      return <FormattedMessage defaultMessage="Zaloguj się" id="auth.login.infoSelfServiceLogin" />
    case InfoSelfServiceLogin.InfoSelfServiceLoginWith:
      return (
        <ProviderMessage
          fallback={
            <FormattedMessage
              defaultMessage="{provider}"
              id="auth.login.infoSelfServiceLoginWith"
              values={context as any}
            />
          }
          provider={(context as any)?.provider}
        />
      )

    case InfoSelfServiceLogin.InfoSelfServiceLoginReAuth:
      return (
        <FormattedMessage
          defaultMessage="Potwierdź tę akcję korzystając z istniejącej metody logowania"
          id="auth.login.infoSelfServiceLoginReAuth"
        />
      )
    case InfoSelfServiceLogin.InfoSelfServiceLoginMFA:
      return (
        <FormattedMessage defaultMessage="Wypełnij kolejny krok logowania" id="auth.login.infoSelfServiceLoginMFA" />
      )
    case InfoSelfServiceLogin.InfoSelfServiceLoginVerify:
      return <FormattedMessage defaultMessage="Zweryfikuj" id="auth.login.infoSelfServiceLoginVerify" />
    case InfoSelfServiceLogin.InfoSelfServiceLoginTOTPLabel:
      return <FormattedMessage defaultMessage="Kod uwierzytelnienia" id="auth.login.infoSelfServiceLoginTOTPLabel" />
    case InfoSelfServiceLogin.InfoLoginLookupLabel:
      return <FormattedMessage defaultMessage="Kod przywracania" id="auth.login.infoLoginLookupLabel" />
    case InfoSelfServiceLogin.InfoSelfServiceLoginWebAuthn:
      return (
        <FormattedMessage defaultMessage="Wprowadź kod bezpieczeństwa" id="auth.login.infoSelfServiceLoginWebAuthn" />
      )
    case InfoSelfServiceLogin.InfoLoginTOTP:
      return <FormattedMessage defaultMessage="Potwierdź i zaloguj się" id="auth.login.infoLoginTOTP" />
    case InfoSelfServiceLogin.InfoLoginLookup:
      return <FormattedMessage defaultMessage="Wprowadź kod przywracania" id="auth.login.infoLoginLookup" />
    case InfoSelfServiceLogin.InfoSelfServiceLoginContinueWebAuthn:
      return (
        <FormattedMessage
          defaultMessage="Kontynuuj z kodem bezpieczeństwa"
          id="auth.login.infoSelfServiceLoginContinueWebAuthn"
        />
      )
    case InfoSelfServiceLogin.InfoSelfServiceLoginWebAuthnPasswordless:
      return (
        <FormattedMessage
          defaultMessage="Przygotuj swoje urządzenie (np. klucz bezpieczeństwa, skaner biometryczny) i kliknij przycisk Kontynuuj"
          id="auth.login.infoSelfServiceLoginWebAuthnPasswordless"
        />
      )
    case InfoSelfServiceLogin.InfoSelfServiceLoginContinue:
      return <FormattedMessage defaultMessage="Kontynuuj" id="auth.login.infoSelfServiceLoginContinue" />
    case 1010021: // InfoSelfServiceLoginPasskey
      return (
        <>
          <ThemeComponent dark={<IconPasskeyDark />} light={<IconPasskeyLight />} />
          <FormattedMessage defaultMessage="Klucz dostępu" id="auth.login.infoSelfServiceLoginPasskey" />
        </>
      )

    case InfoSelfServiceRegistration.InfoSelfServiceRegistration:
      return <FormattedMessage defaultMessage="Zarejestruj się" id="auth.registration.infoSelfServiceRegistration" />
    case InfoSelfServiceRegistration.InfoSelfServiceRegistrationWith:
      return (
        <FormattedMessage
          defaultMessage="Zarejestruj się z {provider}"
          id="auth.registration.infoSelfServiceRegistrationWith"
          values={context as any}
        />
      )
    case InfoSelfServiceRegistration.InfoSelfServiceRegistrationContinue:
      return <FormattedMessage defaultMessage="Kontynuuj" id="auth.registration.infoSelfServiceRegistrationContinue" />
    case InfoSelfServiceRegistration.InfoSelfServiceRegistrationRegisterWebAuthn:
      return (
        <FormattedMessage
          defaultMessage="Zarejestruj się z kluczem bezpieczeństwa"
          id="auth.registration.infoSelfServiceRegistrationRegisterWebAuthn"
        />
      )

    case InfoSelfServiceSettings.InfoSelfServiceSettingsUpdateSuccess:
      return (
        <FormattedMessage
          defaultMessage="Zmiany zostały zapisane"
          id="auth.settings.infoSelfServiceSettingsUpdateSuccess"
        />
      )
    case InfoSelfServiceSettings.InfoSelfServiceSettingsUpdateLinkOidc:
      return (
        <ProviderMessage
          fallback={
            <FormattedMessage
              defaultMessage="{provider}"
              id="auth.settings.infoSelfServiceSettingsUpdateLinkOidc"
              values={context as any}
            />
          }
          provider={(context as any)?.provider}
        />
      )
    case InfoSelfServiceSettings.InfoSelfServiceSettingsUpdateUnlinkOidc:
      return (
        <FormattedMessage
          defaultMessage="Odłącz konto {provider}"
          id="auth.settings.infoSelfServiceSettingsUpdateUnlinkOidc"
          values={context as any}
        />
      )
    case InfoSelfServiceSettings.InfoSelfServiceSettingsUpdateUnlinkTOTP:
      return (
        <FormattedMessage
          defaultMessage="Wyłącz weryfikację dwuetapową"
          id="auth.settings.infoSelfServiceSettingsUpdateUnlinkTOTP"
        />
      )
    case InfoSelfServiceSettings.InfoSelfServiceSettingsTOTPSecretLabel:
      return (
        <FormattedMessage
          defaultMessage="Jeśli nie możesz użyć kodu QR, podaj w aplikacji ten klucz aktywacyjny:"
          id="auth.settings.infoSelfServiceSettingsTOTPSecretLabel"
        />
      )
    case 1050019:
      return (
        <>
          <ThemeComponent dark={<IconPasskeyDark />} light={<IconPasskeyLight />} />
          <FormattedMessage defaultMessage="Klucz dostępu" id="auth.settings.infoSelfServiceSettingsPasskey" />
        </>
      )

    case InfoSelfServiceRecovery.InfoSelfServiceRecoverySuccessful:
      return (
        <FormattedMessage
          defaultMessage="Proces odzyskiwania dostępu został zakończony"
          id="auth.recovery.infoSelfServiceRecoverySuccessful"
        />
      )
    case InfoSelfServiceRecovery.InfoSelfServiceRecoveryEmailSent:
      return (
        <FormattedMessage
          defaultMessage="Na podany adres e-mail wysłaliśmy Ci wiadomość z linkiem do resetowania hasła"
          id="auth.recovery.infoSelfServiceRecoveryEmailSent"
        />
      )
    case InfoSelfServiceRecovery.InfoSelfServiceRecoveryEmailWithCodeSent:
      return (
        <FormattedMessage
          defaultMessage="Wiadomość e-mail z kodem weryfikacyjnym została wysłana na podany przez Ciebie adres"
          id="auth.recovery.infoSelfServiceRecoveryEmailWithCodeSent"
        />
      )

    case InfoNodeLabel.InfoNodeLabelInputPassword:
      return <FormattedMessage defaultMessage="Hasło" id="auth.label.infoNodeLabelInputPassword" />
    case InfoNodeLabel.InfoNodeLabelGenerated:
      switch ((context as any).title) {
        case "Given name":
          return <FormattedMessage defaultMessage="Imię" id="auth.label.infoNodeLabelGivenName" />
        case "Family name":
          return <FormattedMessage defaultMessage="Nazwisko" id="auth.label.infoNodeLabelFamilyName" />
        case "Email":
        case "E-Mail":
          return <FormattedMessage defaultMessage="E-mail" id="auth.label.infoNodeLabelEmail" />
        case "I accept Terms of Service and Privacy Policy":
          return (
            <FormattedMessage
              defaultMessage="Akceptuję regulamin i politykę prywatności"
              id="auth.label.infoNodeLabelRegulations"
            />
          )
      }

      return <FormattedMessage defaultMessage="Nieznane pole" id="auth.label.infoNodeLabelGenerated" />

    case InfoNodeLabel.InfoNodeLabelSave:
      return <FormattedMessage defaultMessage="Zapisz" id="auth.label.infoNodeLabelSave" />
    case InfoNodeLabel.InfoNodeLabelID:
      return <FormattedMessage defaultMessage="Identyfikator" id="auth.label.infoNodeLabelID" />
    case InfoNodeLabel.InfoNodeLabelSubmit:
      return <FormattedMessage defaultMessage="Potwierdź" id="auth.label.infoNodeLabelSubmit" />
    case InfoNodeLabel.InfoNodeLabelVerifyOTP:
      return <FormattedMessage defaultMessage="Kod potwierdzający" id="auth.label.infoNodeLabelVerifyOTP" />
    case InfoNodeLabel.InfoNodeLabelEmail:
      return <FormattedMessage defaultMessage="E-mail" id="auth.label.infoNodeLabelEmail" />
    case InfoNodeLabel.InfoNodeLabelResendOTP:
      return <FormattedMessage defaultMessage="Wyślij kod ponownie" id="auth.label.infoNodeLabelResendOTP" />
    case InfoNodeLabel.InfoNodeLabelContinue:
      return <FormattedMessage defaultMessage="Kontynuuj" id="auth.label.infoNodeLabelContinue" />
    case InfoNodeLabel.InfoNodeLabelRecoveryCode:
      return <FormattedMessage defaultMessage="Kod" id="auth.label.infoNodeLabelRecoveryCode" />
    case InfoNodeLabel.InfoNodeLabelVerificationCode:
      return <FormattedMessage defaultMessage="Kod" id="auth.label.infoNodeLabelVerificationCode" />

    case InfoSelfServiceVerification.InfoSelfServiceVerificationEmailSent:
      return (
        <FormattedMessage
          defaultMessage="Wiadomość e-mail z linkiem weryfikacyjnym została wysłana na podany przez Ciebie adres"
          id="auth.verification.infoSelfServiceVerificationEmailSent"
        />
      )
    case InfoSelfServiceVerification.InfoSelfServiceVerificationSuccessful:
      return (
        <FormattedMessage
          defaultMessage="Twój adres e-mail został potwierdzony"
          id="auth.verification.infoSelfServiceVerificationSuccessful"
        />
      )
    case InfoSelfServiceVerification.InfoSelfServiceVerificationEmailWithCodeSent:
      return (
        <FormattedMessage
          defaultMessage="Wiadomość e-mail z kodem weryfikacyjnym została wysłana na podany przez Ciebie adres"
          id="auth.verification.infoSelfServiceVerificationEmailWithCodeSent"
        />
      )

    case ErrorValidation.ErrorValidationGeneric:
      if ((context as any)?.reason === "Registration is not allowed because it was disabled.")
        return (
          <FormattedMessage
            defaultMessage="Upewnij się, że konto dla Twojego adresu email istnieje, użyj połączonej z Twoim kontem opcji logowania albo skontaktuj się z Administratorem"
            id="auth.error.errorValidationGenericRegistrationDisabled"
          />
        )

      if ((context as any)?.reason.startsWith("Unable to complete OpenID Connect flow "))
        return (
          <FormattedMessage
            defaultMessage="Wprowadzone dane są nieprawidłowe. Sprawdź ich poprawność"
            id="auth.error.errorValidationGenericOpenIDConnect"
          />
        )

      return <FormattedMessage defaultMessage="Pole ma niepoprawny format" id="auth.error.errorValidationGeneric" />
    case ErrorValidation.ErrorValidationRequired:
      return (
        <FormattedMessage
          defaultMessage="To pole jest wymagane"
          id="auth.error.errorValidationRequired"
          values={context as any}
        />
      )
    case ErrorValidation.ErrorValidationMinLength:
    case ErrorValidation.ErrorValidationInvalidFormat:
      return (
        <FormattedMessage
          defaultMessage="Niepoprawny format danych ({expected_format})"
          id="auth.error.errorValidationFormat"
          values={context as any}
        />
      )

    case ErrorValidation.ErrorValidationPasswordPolicyViolation:
      return (
        <FormattedMessage
          defaultMessage="Min. 8 znaków{br}Nie może być na liście haseł, które wyciekły. Sprawdź na <leakedPasswordsLink>haveibeenpwned</leakedPasswordsLink>{br}Nie może być zbyt podobne do adresu e-mail"
          id="auth.error.errorValidationPasswordPolicyViolation"
          values={{
            br: <br />,
            leakedPasswordsLink: chunks => (
              <a href="https://haveibeenpwned.com/Passwords" rel="noreferrer" target="_blank">
                {chunks}
              </a>
            ),
          }}
        />
      )
    case ErrorValidation.ErrorValidationInvalidCredentials:
      return (
        <FormattedMessage
          defaultMessage="Email lub hasło nieprawidłowe. Spróbuj użyć przypomnienia hasła albo skontaktuj się ze swoim opiekunem w Develii"
          id="auth.error.errorValidationInvalidCredentials"
        />
      )
    case ErrorValidation.ErrorValidationDuplicateCredentials:
      return (
        <FormattedMessage
          defaultMessage="Konto z takimi danymi logowania już istnieje"
          id="auth.error.errorValidationDuplicateCredentials"
        />
      )
    case ErrorValidation.ErrorValidationTOTPVerifierWrong:
      return (
        <FormattedMessage
          defaultMessage="Podany kod weryfikacyjny jest nieprawidłowy"
          id="auth.error.errorValidationTOTPVerifierWrong"
        />
      )
    case ErrorValidation.ErrorValidationIdentifierMissing:
      return (
        <FormattedMessage
          defaultMessage="Nie znaleziono żadnych danych logowania"
          id="auth.error.errorValidationIdentifierMissing"
        />
      )
    case ErrorValidation.ErrorValidationAddressNotVerified:
      return (
        <FormattedMessage
          defaultMessage="Potwierdź swój adres e-mail"
          id="auth.error.errorValidationAddressNotVerified"
        />
      )
    case ErrorValidation.ErrorValidationNoTOTPDevice:
      return (
        <FormattedMessage
          defaultMessage="Nie wykryto żadnego urządzenia TOTP do uwierzytelnienia"
          id="auth.error.errorValidationNoTOTPDevice"
        />
      )
    case ErrorValidation.ErrorValidationLookupAlreadyUsed:
      return (
        <FormattedMessage
          defaultMessage="Kod przywracania został już wykorzystany"
          id="auth.error.errorValidationLookupAlreadyUsed"
        />
      )
    case ErrorValidation.ErrorValidationNoWebAuthnDevice:
      return (
        <FormattedMessage
          defaultMessage="Nie wykryto żadnego urządzenia do uwierzytelnienia"
          id="auth.error.errorValidationNoWebAuthnDevice"
        />
      )
    case ErrorValidation.ErrorValidationNoLookup:
      return (
        <FormattedMessage
          defaultMessage="Nie posiadasz ustawionych kodów przywracania"
          id="auth.error.errorValidationNoLookup"
        />
      )
    case ErrorValidation.ErrorValidationSuchNoWebAuthnUser:
      return (
        <FormattedMessage
          defaultMessage="Takie konto nie istnieje lub nie ma jeszcze ustawionych kodów uwierzytelnienia"
          id="auth.error.errorValidationSuchNoWebAuthnUser"
        />
      )
    case ErrorValidation.ErrorValidationLookupInvalid:
      return (
        <FormattedMessage
          defaultMessage="Kod przywracania jest niepoprawny"
          id="auth.error.errorValidationLookupInvalid"
        />
      )

    case ErrorValidationRecovery.ErrorValidationRecoveryRetrySuccess:
      return (
        <FormattedMessage
          defaultMessage="Zapytanie zostało już wysłane poprawnie i nie może zostać powtórzone"
          id="auth.error.errorValidationRecoveryRetrySuccess"
        />
      )
    case ErrorValidationRecovery.ErrorValidationRecoveryStateFailure:
      return (
        <FormattedMessage
          defaultMessage="Wystąpił błąd w trakcie przywracania. Spróbuj ponownie"
          id="auth.error.errorValidationRecoveryStateFailure"
        />
      )
    case ErrorValidationRecovery.ErrorValidationRecoveryMissingRecoveryToken:
      return (
        <FormattedMessage
          defaultMessage="Kod jest nieprawidłowy"
          id="auth.error.errorValidationRecoveryMissingRecoveryToken"
        />
      )
    case ErrorValidationRecovery.ErrorValidationRecoveryTokenInvalidOrAlreadyUsed:
      return (
        <FormattedMessage
          defaultMessage="Kod jest nieprawidłowy lub został już wykorzystany"
          id="auth.error.errorValidationRecoveryTokenInvalidOrAlreadyUsed"
        />
      )
    case ErrorValidationRecovery.ErrorValidationRecoveryFlowExpired:
      return (
        <FormattedMessage
          defaultMessage="Link do aktywacji wygasł"
          id="auth.error.errorValidationRecoveryFlowExpired"
        />
      )
    case ErrorValidationRecovery.ErrorValidationRecoveryCodeInvalidOrAlreadyUsed:
      return (
        <FormattedMessage
          defaultMessage="Kod jest nieprawidłowy lub został już wykorzystany"
          id="auth.error.errorValidationRecoveryCodeInvalidOrAlreadyUsed"
        />
      )

    case ErrorValidationVerification.ErrorValidationVerificationTokenInvalidOrAlreadyUsed:
      return (
        <FormattedMessage
          defaultMessage="Kod jest nieprawidłowy lub został już wykorzystany"
          id="auth.error.errorValidationVerificationTokenInvalidOrAlreadyUsed"
        />
      )
    case ErrorValidationVerification.ErrorValidationVerificationRetrySuccess:
      return (
        <FormattedMessage
          defaultMessage="Zapytanie zostało już wysłane poprawnie i nie może zostać powtórzone"
          id="auth.error.errorValidationVerificationRetrySuccess"
        />
      )
    case ErrorValidationVerification.ErrorValidationVerificationStateFailure:
      return (
        <FormattedMessage
          defaultMessage="Wystąpił błąd w trakcie weryfikacji. Spróbuj ponownie"
          id="auth.error.errorValidationVerificationStateFailure"
        />
      )
    case ErrorValidationVerification.ErrorValidationVerificationMissingVerificationToken:
      return (
        <FormattedMessage
          defaultMessage="Kod jest nieprawidłowy"
          id="auth.error.errorValidationVerificationMissingVerificationToken"
        />
      )
    case ErrorValidationVerification.ErrorValidationVerificationFlowExpired:
      return <FormattedMessage defaultMessage="Link wygasł" id="auth.error.errorValidationVerificationFlowExpired" />
    case ErrorValidationVerification.ErrorValidationVerificationCodeInvalidOrAlreadyUsed:
      return (
        <FormattedMessage
          defaultMessage="Podany kod jest niepoprawny lub został już użyty. Spróbuj ponownie"
          id="auth.error.errorValidationVerificationCodeInvalidOrAlreadyUsed"
        />
      )
  }

  return <>{text}</>
}
