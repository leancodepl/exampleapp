import { useCallback, useMemo } from "react"
import { FormattedMessage } from "react-intl"
import { ContinueWith, LoginFlow, UiNode } from "@ory/client"
import { useNavigate } from "@tanstack/react-router"
import { Separator, SkeletonLoader, Text } from "@web/design-system"
import { RegistrationCard, useRegisterFlow } from "@leancodepl/kratos"
import { useBoundRunInTask } from "@leancodepl/utils"
import { kratosClient } from "../.."
import { AuthContainer } from "../../components/AuthContainer"
import { AuthContextProvider } from "../../utils/authContext"
import { cssRegistrationCard, OidcProviders } from "./styles"

type RegistrationFlowSearchParams = Exclude<Parameters<typeof useRegisterFlow>[0]["searchParams"], undefined>

type RegisterProps = {
  searchParams: RegistrationFlowSearchParams
  onShowVerificationUi: (verificationFlowId: string) => void
}

export function Register({ searchParams, onShowVerificationUi }: RegisterProps) {
  const nav = useNavigate()

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
            onShowVerificationUi(continuation.flow.id)
            break
        }
      }
    },
    [nav, onShowVerificationUi],
  )

  const { flow, submit } = useRegisterFlow({
    kratosClient,
    onSessionAlreadyAvailable: useCallback(() => nav({ to: "/" }), [nav]),
    updateSearchParams: search => nav({ to: "/registration", search }),
    searchParams,
    onContinueWith,
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

  const needsSeparator =
    (passwordFlow?.ui.nodes.length ?? 0) > 2 &&
    ((appleFlow?.ui.nodes.length ?? 0) > 0 ||
      (facebookFlow?.ui.nodes.length ?? 0) > 0 ||
      (googleFlow?.ui.nodes.length ?? 0) > 0 ||
      (passkeyFlow?.ui.nodes.length ?? 0) > 2)

  return (
    <AuthContainer>
      <Text headline-large color="default.primary">
        <FormattedMessage defaultMessage="Rejestracja" id="auth.register.title.main" />
      </Text>

      {passwordFlow ? (
        <AuthContextProvider isLoading={isLoadingPassword}>
          <RegistrationCard className={cssRegistrationCard} flow={passwordFlow} onSubmit={submitPassword} />
        </AuthContextProvider>
      ) : (
        <SkeletonLoader height={230} />
      )}

      {needsSeparator && (
        <Separator>
          <FormattedMessage defaultMessage="lub zarejestruj się przy użyciu" id="auth.register.label.orRegisterWith" />
        </Separator>
      )}

      <OidcProviders>
        {facebookFlow ? (
          <AuthContextProvider isLoading={isLoadingFacebook}>
            <RegistrationCard flow={facebookFlow} onSubmit={submitFacebook} />
          </AuthContextProvider>
        ) : (
          <SkeletonLoader height={40} />
        )}

        {appleFlow ? (
          <AuthContextProvider isLoading={isLoadingApple}>
            <RegistrationCard flow={appleFlow} onSubmit={submitApple} />
          </AuthContextProvider>
        ) : (
          <SkeletonLoader height={40} />
        )}

        {googleFlow ? (
          <AuthContextProvider isLoading={isLoadingGoogle}>
            <RegistrationCard flow={googleFlow} onSubmit={submitGoogle} />
          </AuthContextProvider>
        ) : (
          <SkeletonLoader height={40} />
        )}

        {passkeyFlow ? (
          <AuthContextProvider isLoading={isLoadingPasskey}>
            <RegistrationCard flow={passkeyFlow} onSubmit={submitPasskey} />
          </AuthContextProvider>
        ) : (
          <SkeletonLoader height={40} />
        )}
      </OidcProviders>
    </AuthContainer>
  )
}

function filterFlowNodes(flow: LoginFlow | undefined, filter: (node: UiNode) => boolean) {
  if (!flow) return undefined

  return {
    ...flow,
    ui: {
      ...flow.ui,
      nodes: flow.ui.nodes.filter(filter),
    },
  }
}

function getPasswordFlow(flow?: LoginFlow) {
  return filterFlowNodes(
    flow,
    node => node.group === "password" || node.group === "profile" || node.group === "default",
  )
}

function getOidcFlow(flow: LoginFlow | undefined, provider: "apple" | "facebook" | "google") {
  return filterFlowNodes(
    flow,
    node => node.group === "oidc" && node.attributes.node_type === "input" && node.attributes.value === provider,
  )
}

function getPasskeyFlow(flow?: LoginFlow) {
  return filterFlowNodes(
    flow,
    node =>
      node.group === "passkey" ||
      node.group === "webauthn" ||
      (node.group === "default" && node.attributes.node_type === "input" && node.attributes.type === "hidden"),
  )
}

function skipMessages(flow?: LoginFlow): LoginFlow | undefined {
  if (!flow) return undefined

  return {
    ...flow,
    ui: {
      ...flow.ui,
      messages: undefined,
    },
  }
}
