import { KratosComponents } from "@leancodepl/kratos"
import { ButtonComponent } from "./components/Button"
import { CheckboxComponent } from "./components/Checkbox"
import { InputComponent } from "./components/Input"
import { MessageFormat } from "./components/MessageFormat"
import { OidcSectionWrapper } from "./components/OidcSectionWrapper"
import { TextComponent } from "./components/Text"
import { UiMessages } from "./components/UiMessages"

export const kratosComponents: Partial<KratosComponents> = {
    MessageFormat: MessageFormat,
    Text: TextComponent,
    Input: InputComponent,
    Button: ButtonComponent,
    Checkbox: CheckboxComponent,
    UiMessages,
    OidcSectionWrapper,
}
