import { useCallback, useEffect, useMemo } from "react"
import { FormattedMessage } from "react-intl"
import { ContinueWith, SettingsFlow, UiNode } from "@ory/client"
import { Link, useNavigate } from "@tanstack/react-router"
import { Button, Flex, SkeletonLoader, Text, useIsMobile } from "@web/design-system"
import { IconChevronLeft } from "@web/design-system/icons"
import { KratosContextProvider, UserSettingsCard } from "@leancodepl/kratos"
import { useBoundRunInTask } from "@leancodepl/utils"
import { kratosClient } from "../.."
import { Title } from "../../../components/AppTitle"
import { ProfileListItems } from "../../../components/Profile/ProfileListItem"
import { AuthContainer } from "../../components/AuthContainer"
import { ProviderMessage } from "../../components/MessageFormat/ProviderMessage"
import { useSettingsFlow } from "../../hooks/useSettingsFlow"
import { sessionManager } from "../../sessionManager"
import { AuthContextProvider } from "../../utils/authContext"
import { profilePasskeysComponents } from "../../utils/profilePasskeysComponents"
import { profileSocialsComponents } from "../../utils/profileSocialsComponents"
import { SlimProfileListItem } from "./styles"

export function ProfileOidcFlow() {
  const isMobile = useIsMobile()
  const nav = useNavigate({ from: "/" })

  const onContinueWith = useCallback(
    (continueWith: ContinueWith[]) => {
      for (const continuation of continueWith) {
        switch (continuation.action) {
          case "redirect_browser_to":
            window.location.href = continuation.redirect_browser_to
            break
          case "show_recovery_ui":
            nav({ to: "/recovery", search: { flow: continuation.flow.id } })
            break
          case "show_settings_ui":
            break
          case "show_verification_ui":
            break
        }
      }
    },
    [nav],
  )

  const { flow, submit } = useSettingsFlow({
    kratosClient,
    onContinueWith,
    returnTo: "/profile",
  })

  const { appleFlow, facebookFlow, googleFlow, passkeyFlow } = useMemo(
    () => ({
      appleFlow: skipMessages(getOidcFlow(flow, "apple")),
      facebookFlow: skipMessages(getOidcFlow(flow, "facebook")),
      googleFlow: skipMessages(getOidcFlow(flow, "google")),
      passkeyFlow: skipMessages(getPasskeyFlow(flow)),
    }),
    [flow],
  )

  const [isLoadingFacebook, submitFacebook] = useBoundRunInTask(submit)
  const [isLoadingApple, submitApple] = useBoundRunInTask(submit)
  const [isLoadingGoogle, submitGoogle] = useBoundRunInTask(submit)
  const [isLoadingPasskey, submitPasskey] = useBoundRunInTask(submit)

  useEffect(() => {
    if (!flow) return

    if (flow.state === "success") {
      sessionManager.checkIfLoggedIn()
      nav({ to: "/" })
    }
  }, [flow, nav])

  return (
    <AuthContainer>
      {isMobile && (
        <Title>
          <Flex align="center" gap="small">
            <Button asChild leading={<IconChevronLeft />} type="text">
              <Link to="/profile" />
            </Button>
            <FormattedMessage defaultMessage="Metody logowania" id="auth.profile.title.loginMethods" />
          </Flex>
        </Title>
      )}

      <Text headline-small color="default.primary">
        <FormattedMessage defaultMessage="Połączone konta" id="auth.profile.title.connectedAccounts" />
      </Text>

      <KratosContextProvider components={profileSocialsComponents}>
        <ProfileListItems>
          <SlimProfileListItem>
            <Flex gap="small" justify="space-between">
              <Text asChild body color="default.primary">
                <Flex align="center" gap="small">
                  <ProviderMessage provider="facebook" />
                </Flex>
              </Text>

              {facebookFlow ? (
                <AuthContextProvider isLoading={isLoadingFacebook}>
                  <UserSettingsCard flow={facebookFlow} flowType="oidc" onSubmit={submitFacebook} />
                </AuthContextProvider>
              ) : (
                <SkeletonLoader height={40} />
              )}
            </Flex>
          </SlimProfileListItem>

          <SlimProfileListItem>
            <Flex gap="small" justify="space-between">
              <Text asChild body color="default.primary">
                <Flex align="center" gap="small">
                  <ProviderMessage provider="apple" />
                </Flex>
              </Text>

              {appleFlow ? (
                <AuthContextProvider isLoading={isLoadingApple}>
                  <UserSettingsCard flow={appleFlow} flowType="oidc" onSubmit={submitApple} />
                </AuthContextProvider>
              ) : (
                <SkeletonLoader height={40} />
              )}
            </Flex>
          </SlimProfileListItem>

          <SlimProfileListItem>
            <Flex gap="small" justify="space-between">
              <Text asChild body color="default.primary">
                <Flex align="center" gap="small">
                  <ProviderMessage provider="google" />
                </Flex>
              </Text>

              {googleFlow ? (
                <AuthContextProvider isLoading={isLoadingGoogle}>
                  <UserSettingsCard flow={googleFlow} flowType="oidc" onSubmit={submitGoogle} />
                </AuthContextProvider>
              ) : (
                <SkeletonLoader height={40} />
              )}
            </Flex>
          </SlimProfileListItem>
        </ProfileListItems>
      </KratosContextProvider>

      <Text headline-small color="default.primary">
        <FormattedMessage defaultMessage="Klucze dostępu" id="auth.profile.title.accessKeys" />
      </Text>

      {passkeyFlow ? (
        <KratosContextProvider components={profilePasskeysComponents}>
          <AuthContextProvider isLoading={isLoadingPasskey}>
            <UserSettingsCard flow={passkeyFlow} flowType="passkey" onSubmit={submitPasskey} />
          </AuthContextProvider>
        </KratosContextProvider>
      ) : (
        <SkeletonLoader height={40} />
      )}

      <Text headline-small color="default.primary">
        <FormattedMessage defaultMessage="Aplikacje do uwierzytelnienia" id="auth.profile.title.authApps" />
      </Text>

      {flow ? (
        <AuthContextProvider isLoading={isLoadingPasskey}>
          <UserSettingsCard flow={flow} flowType="totp" onSubmit={submitPasskey} />
        </AuthContextProvider>
      ) : (
        <SkeletonLoader height={40} />
      )}
    </AuthContainer>
  )
}

function filterFlowNodes(flow: SettingsFlow | undefined, filter: (node: UiNode) => boolean) {
  if (!flow) return undefined

  return {
    ...flow,
    ui: {
      ...flow.ui,
      nodes: flow.ui.nodes.filter(filter),
    },
  }
}

function getOidcFlow(flow: SettingsFlow | undefined, provider: "apple" | "facebook" | "google") {
  return filterFlowNodes(
    flow,
    node => node.group === "oidc" && node.attributes.node_type === "input" && node.attributes.value === provider,
  )
}

function getPasskeyFlow(flow?: SettingsFlow) {
  const newFlow = filterFlowNodes(
    flow,
    node =>
      node.group === "passkey" ||
      node.group === "webauthn" ||
      (node.group === "default" && node.attributes.node_type === "input" && node.attributes.type === "hidden"),
  )

  if (!newFlow) return undefined

  return {
    ...newFlow,
    ui: {
      ...newFlow.ui,
      nodes: newFlow.ui.nodes.toReversed(),
    },
  }
}

function skipMessages(flow?: SettingsFlow): SettingsFlow | undefined {
  if (!flow) return undefined

  return {
    ...flow,
    ui: {
      ...flow.ui,
      messages: undefined,
    },
  }
}
