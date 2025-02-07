import { KratosComponents } from "@leancodepl/kratos"
import { ButtonComponent } from "./components/Button"
import { CheckboxComponent } from "./components/Checkbox"
import { InputComponent } from "./components/Input"
import { MessageFormat } from "./components/MessageFormat"
import { OidcSectionWrapper } from "./components/OidcSectionWrapper"
import { OidcSettingsSectionWrapper } from "./components/OidcSettingsSectionWrapper"
import { PasswordlessSectionWrapper } from "./components/PasswordlessSectionWrapper"
import { RegistrationSectionWrapper } from "./components/RegistrationSectionWrapper"
import { TextComponent } from "./components/Text"
import { UiMessages } from "./components/UiMessages"
import { WebAuthnSettingsSectionWrapper } from "./components/WebAuthnSettingsSectionWrapper"

export const kratosComponents: Partial<KratosComponents> = {
    MessageFormat: MessageFormat,
    Text: TextComponent,
    Input: InputComponent,
    Button: ButtonComponent,
    Checkbox: CheckboxComponent,
    UiMessages,
    OidcSectionWrapper,
    OidcSettingsSectionWrapper,
    PasswordlessSectionWrapper,
    WebAuthnSettingsSectionWrapper,
    RegistrationSectionWrapper,
}
