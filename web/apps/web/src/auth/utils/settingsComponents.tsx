import { FormattedMessage } from "react-intl"
import { InfoNodeLabel, KratosComponents } from "@leancodepl/kratos"
import { MessageFormat } from "../components/MessageFormat"

export const settingsComponents: Partial<KratosComponents> = {
  MessageFormat: ({ id, context, text }) => {
    switch (id) {
      case InfoNodeLabel.InfoNodeLabelSave:
        return <FormattedMessage defaultMessage="Ustaw hasło" id="auth.settings.action.setPassword" />
    }

    return <MessageFormat context={context} id={id} text={text} />
  },
}
