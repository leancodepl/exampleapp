import { KratosComponents } from "@leancodepl/kratos"
import { AuthButton } from "./components/AuthButton"
import { AuthCheckbox } from "./components/AuthCheckbox"
import { AuthInput } from "./components/AuthInput"
import { AuthLink } from "./components/AuthLink"
import { AuthUiMessages } from "./components/AuthUiMessages"
import { MessageFormat } from "./components/MessageFormat"
import { SectionWrapper } from "./components/SectionWrapper"

export const kratosComponents: Partial<KratosComponents> = {
  UiMessages: AuthUiMessages,
  MessageFormat: MessageFormat,
  Input: AuthInput,
  Button: AuthButton,
  Checkbox: AuthCheckbox,
  Link: AuthLink,
  LoginSectionWrapper: SectionWrapper,
  OidcSectionWrapper: SectionWrapper,
  PasswordlessSectionWrapper: SectionWrapper,
  AuthCodeSectionWrapper: SectionWrapper,
  RegistrationSectionWrapper: SectionWrapper,
  LinkSectionWrapper: SectionWrapper,
  ProfileSettingsSectionWrapper: SectionWrapper,
  PasswordSettingsSectionWrapper: SectionWrapper,
  WebAuthnSettingsSectionWrapper: SectionWrapper,
  LookupSecretSettingsSectionWrapper: SectionWrapper,
  OidcSettingsSectionWrapper: SectionWrapper,
  TotpSettingsSectionWrapper: SectionWrapper,
  IdentifierFirstLoginSectionWrapper: SectionWrapper,
  ProfileLoginSectionWrapper: SectionWrapper,
  ProfileRegistrationSectionWrapper: SectionWrapper,
}
