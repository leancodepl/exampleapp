import { useEffect } from "react"
import { UiTextTypeEnum } from "@ory/client"
import { App } from "antd"
import { UiMessagesComponentProps } from "@leancodepl/kratos"

export function UiMessages({ uiMessages }: UiMessagesComponentProps) {
    const { message } = App.useApp()

    useEffect(() => {
        uiMessages?.forEach(({ type, text }) => {
            switch (type) {
                case UiTextTypeEnum.Info:
                    message.info(text)
                    return
                case UiTextTypeEnum.Success:
                    message.success(text)
                    return
                case UiTextTypeEnum.Error:
                    message.error(text)
                    return
            }
        })
    }, [message, uiMessages])

    return null
}
