import { useCallback, useEffect, useMemo } from "react"
import { FormattedMessage } from "react-intl"
import { ContinueWith, SettingsFlow, UiNode } from "@ory/client"
import { useNavigate } from "@tanstack/react-router"
import { Separator, SkeletonLoader, Text } from "@web/design-system"
import { KratosContextProvider, UserSettingsCard } from "@leancodepl/kratos"
import { useBoundRunInTask } from "@leancodepl/utils"
import { kratosClient } from "../.."
import { AuthContainer } from "../../components/AuthContainer"
import { useSettingsFlow } from "../../hooks/useSettingsFlow"
import { sessionManager } from "../../sessionManager"
import { AuthContextProvider } from "../../utils/authContext"
import { settingsComponents } from "../../utils/settingsComponents"
import { OidcProviders } from "../Login/styles"

export function OidcFlow() {
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

  const { passwordFlow, appleFlow, facebookFlow, googleFlow, passkeyFlow } = useMemo(
    () => ({
      passwordFlow: getPasswordFlow(flow),
      appleFlow: skipMessages(getOidcFlow(flow, "apple")),
      facebookFlow: skipMessages(getOidcFlow(flow, "facebook")),
      googleFlow: skipMessages(getOidcFlow(flow, "google")),
      passkeyFlow: skipMessages(getPasskeyFlow(flow)),
    }),
    [flow],
  )

  const [isLoadingPassword, submitPassword] = useBoundRunInTask(submit)
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
      <Text headline-large color="default.primary">
        <FormattedMessage defaultMessage="Ustaw sposób logowania" id="auth.oidc.title.main" />
      </Text>

      {passwordFlow ? (
        <KratosContextProvider components={settingsComponents}>
          <AuthContextProvider isLoading={isLoadingPassword}>
            <UserSettingsCard flow={passwordFlow} flowType="password" onSubmit={submitPassword} />
          </AuthContextProvider>
        </KratosContextProvider>
      ) : (
        <SkeletonLoader height={230} />
      )}

      <Separator>
        <FormattedMessage defaultMessage="lub zaloguj się przy użyciu" id="auth.oidc.label.orLoginWith" />
      </Separator>

      <OidcProviders>
        {facebookFlow ? (
          <AuthContextProvider isLoading={isLoadingFacebook}>
            <UserSettingsCard flow={facebookFlow} flowType="oidc" onSubmit={submitFacebook} />
          </AuthContextProvider>
        ) : (
          <SkeletonLoader height={40} />
        )}

        {appleFlow ? (
          <AuthContextProvider isLoading={isLoadingApple}>
            <UserSettingsCard flow={appleFlow} flowType="oidc" onSubmit={submitApple} />
          </AuthContextProvider>
        ) : (
          <SkeletonLoader height={40} />
        )}

        {googleFlow ? (
          <AuthContextProvider isLoading={isLoadingGoogle}>
            <UserSettingsCard flow={googleFlow} flowType="oidc" onSubmit={submitGoogle} />
          </AuthContextProvider>
        ) : (
          <SkeletonLoader height={40} />
        )}

        {passkeyFlow ? (
          <AuthContextProvider isLoading={isLoadingPasskey}>
            <UserSettingsCard flow={passkeyFlow} flowType="passkey" onSubmit={submitPasskey} />
          </AuthContextProvider>
        ) : (
          <SkeletonLoader height={40} />
        )}
      </OidcProviders>
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

function getPasswordFlow(flow?: SettingsFlow) {
  return filterFlowNodes(flow, node => node.group === "password" || node.group === "default")
}

function getOidcFlow(flow: SettingsFlow | undefined, provider: "apple" | "facebook" | "google") {
  return filterFlowNodes(
    flow,
    node => node.group === "oidc" && node.attributes.node_type === "input" && node.attributes.value === provider,
  )
}

function getPasskeyFlow(flow?: SettingsFlow) {
  return filterFlowNodes(
    flow,
    node =>
      node.group === "passkey" ||
      node.group === "webauthn" ||
      (node.group === "default" && node.attributes.node_type === "input" && node.attributes.type === "hidden"),
  )
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
