import { FormattedMessage } from "react-intl"
import { InfoNodeLabel, KratosComponents } from "@leancodepl/kratos"
import { MessageFormat } from "../components/MessageFormat"

export const passwordComponents: Partial<KratosComponents> = {
  MessageFormat: ({ id, context, text }) => {
    switch (id) {
      case InfoNodeLabel.InfoNodeLabelInputPassword:
        return <FormattedMessage defaultMessage="Nowe hasło" id="auth.password.label.newPassword" />
      case InfoNodeLabel.InfoNodeLabelSave:
        return <FormattedMessage defaultMessage="Ustaw hasło" id="auth.password.action.setPassword" />
    }

    return <MessageFormat context={context} id={id} text={text} />
  },
}
