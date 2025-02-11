import { useEffect, useRef } from "react"
import { Snackbar } from "@web/design-system"
import { UiMessagesComponentProps } from "@leancodepl/kratos"
import { MessageFormat } from "../MessageFormat"

export function AuthUiMessages({ uiMessages }: UiMessagesComponentProps) {
  const { error, success, info } = Snackbar.use()
  const message = useRef<Snackbar.MountedSnackbar>(null)

  useEffect(() => {
    message.current?.close()

    if (uiMessages && uiMessages.length > 0) {
      ;(async () => {
        const title = (
          <>
            {uiMessages.map(message => (
              <MessageFormat key={message.id} context={message.context} id={message.id} text={message.text} />
            ))}
          </>
        )

        if (uiMessages.some(message => message.type === "error")) {
          message.current = await error({ title })
        } else if (uiMessages.some(message => message.type === "success")) {
          message.current = await success({ title })
        } else {
          message.current = await info({ title })
        }
      })()
    }
  }, [error, info, success, uiMessages])

  return null
}
