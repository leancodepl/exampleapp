import { useMemo } from "react"
import { FormattedMessage } from "react-intl"
import { Button, Flex, Text } from "@web/design-system"
import { IconCheck } from "@web/design-system/icons"
import { ButtonComponentProps, InfoSelfServiceSettings, KratosComponents } from "@leancodepl/kratos"
import { ThemeComponent } from "../../components/ThemeComponent"
import { MessageFormat } from "../components/MessageFormat"
import IconPasskeyDark from "../components/MessageFormat/icon_passkey_dark.svg?react"
import IconPasskeyLight from "../components/MessageFormat/icon_passkey_light.svg?react"
import { SlimProfileListItem } from "../flows/ProfileOidc/styles"
import { useAuthContext } from "./authContext"
import { ProfileOidcTick, ProfileOidcTickChecked } from "./styles"

export const profilePasskeysComponents: Partial<KratosComponents> = {
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

    if (node.meta.label?.id === 1050019) {
      return (
        <Flex padding={{ top: "large" }}>
          <Button {...props} htmlType={type} isLoading={isLoading} type="secondary" onClick={onClick} onLoad={onLoad}>
            {header}
          </Button>
        </Flex>
      )
    }

    return (
      <SlimProfileListItem>
        <Flex justify="space-between">
          <Text asChild body color="default.primary">
            <Flex align="center" gap="small">
              <FormattedMessage defaultMessage="Klucz dostępu" id="auth.profile.passkeys.label.accessKey" />
            </Flex>
          </Text>

          <Button {...props} block htmlType={type} isLoading={isLoading} type="text" onClick={onClick} onLoad={onLoad}>
            {header}
          </Button>
        </Flex>
      </SlimProfileListItem>
    )
  },

  WebAuthnSettingsSectionWrapper: ({ children }) => {
    return <div>{children}</div>
  },

  MessageFormat: ({ id, context, text }) => {
    switch (id) {
      case InfoSelfServiceSettings.InfoSelfServiceSettingsUpdateLinkOidc:
        return (
          <Flex gap="small">
            <FormattedMessage defaultMessage="Połącz" id="auth.profile.passkeys.action.connect" />

            <ProfileOidcTick />
          </Flex>
        )
      case InfoSelfServiceSettings.InfoSelfServiceSettingsUpdateUnlinkOidc:
        return (
          <Flex gap="small">
            <FormattedMessage defaultMessage="Odłącz" id="auth.profile.passkeys.action.disconnect" />

            <ProfileOidcTickChecked>
              <IconCheck size="small" />
            </ProfileOidcTickChecked>
          </Flex>
        )
      case 1050019:
        return (
          <>
            <ThemeComponent dark={<IconPasskeyDark />} light={<IconPasskeyLight />} />
            <FormattedMessage defaultMessage="Dodaj klucz dostępu" id="auth.profile.passkeys.action.addAccessKey" />
          </>
        )
      case 1050020:
        return (
          <span>
            <Text ellipsis color="default.secondary">
              ({(context as any)?.display_name})
            </Text>{" "}
            <FormattedMessage defaultMessage="Usuń" id="auth.profile.passkeys.action.delete" />
          </span>
        )
    }

    return <MessageFormat context={context} id={id} text={text} />
  },
}
