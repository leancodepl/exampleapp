import { useMemo } from "react"
import { FormattedMessage } from "react-intl"
import { Button, Flex } from "@web/design-system"
import { IconCheck } from "@web/design-system/icons"
import { ButtonComponentProps, InfoSelfServiceSettings, KratosComponents } from "@leancodepl/kratos"
import { MessageFormat } from "../components/MessageFormat"
import { useAuthContext } from "./authContext"
import { ProfileOidcTick, ProfileOidcTickChecked } from "./styles"

export const profileSocialsComponents: Partial<KratosComponents> = {
  Button: function AuthButton({ header, fullWidth: _fullWidth, node, type, ...props }: ButtonComponentProps) {
    const { onClick, onLoad } = useMemo(() => {
      if (node.attributes.node_type !== "input") return {}

      const onclick = node.attributes.onclick
      const onload = node.attributes.onload

      return {
        onClick: onclick
          ? () => {
              // eslint-disable-next-line no-eval
              eval(onclick)
            }
          : undefined,

        // eslint-disable-next-line no-eval
        onLoad: onload ? () => eval(onload) : undefined,
      }
    }, [node])

    const { isLoading } = useAuthContext()

    return (
      <Button {...props} block htmlType={type} isLoading={isLoading} type="text" onClick={onClick} onLoad={onLoad}>
        {header}
      </Button>
    )
  },

  MessageFormat: ({ id, context, text }) => {
    switch (id) {
      case InfoSelfServiceSettings.InfoSelfServiceSettingsUpdateLinkOidc:
        return (
          <Flex gap="small">
            <FormattedMessage defaultMessage="Połącz" id="auth.profile.socials.action.connect" />

            <ProfileOidcTick />
          </Flex>
        )
      case InfoSelfServiceSettings.InfoSelfServiceSettingsUpdateUnlinkOidc:
        return (
          <Flex gap="small">
            <FormattedMessage defaultMessage="Odłącz" id="auth.profile.socials.action.disconnect" />

            <ProfileOidcTickChecked>
              <IconCheck size="small" />
            </ProfileOidcTickChecked>
          </Flex>
        )
    }

    return <MessageFormat context={context} id={id} text={text} />
  },
}
