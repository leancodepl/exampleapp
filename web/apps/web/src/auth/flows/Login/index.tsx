import { useCallback, useMemo } from "react"
import { FormattedMessage } from "react-intl"
import { AuthenticatorAssuranceLevel, LoginFlow, UiNode } from "@ory/client"
import { Link, Navigate, useNavigate } from "@tanstack/react-router"
import { Button, Separator, SkeletonLoader, Text } from "@web/design-system"
import { LoginCard, LoginSearchParams, returnToParameterName, useLogoutFlow } from "@leancodepl/kratos"
import { useBoundRunInTask } from "@leancodepl/utils"
import { kratosClient } from "../.."
import { Route as LoginRoute } from "../../../routes/_auth/login"
import { AuthContainer } from "../../components/AuthContainer"
import { useLoginFlow } from "../../hooks/useLoginFlow"
import { sessionManager } from "../../sessionManager"
import { AuthContextProvider } from "../../utils/authContext"
import { OidcProviders } from "./styles"

type LoginProps = {
  searchParams: LoginSearchParams
  isLoggedIn: boolean
}

export function Login({ searchParams, isLoggedIn }: LoginProps) {
  const handleLogin = useHandleLogin(searchParams[returnToParameterName])
  const nav = useNavigate()

  const { logout } = useLogoutFlow({
    kratosClient,
    onLoggedOut: useCallback(() => nav({ to: "/" }), [nav]),
  })

  const { flow, submit } = useLoginFlow({
    kratosClient,
    onLoggedIn: handleLogin,
    onSessionAlreadyAvailable: useCallback(() => nav({ to: "/" }), [nav]),
    searchParams,
    updateSearchParams: search => nav({ to: LoginRoute.fullPath, search }),
  })

  const isRefresh = flow?.refresh
  const is2FA = flow?.requested_aal === AuthenticatorAssuranceLevel.Aal2

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

  if (isRefresh && flow.ui.nodes.length <= 1) {
    return <Navigate replace to="/login/expired" />
  }

  if (isRefresh === false && isLoggedIn) {
    return <Navigate replace to={searchParams[returnToParameterName] ?? "/"} />
  }

  return (
    <AuthContainer>
      <Text headline-large color="default.primary">
        {isRefresh ? (
          <FormattedMessage defaultMessage="Potwierdź akcję" id="auth.login.title.reAuthenticate" />
        ) : (
          <FormattedMessage defaultMessage="Logowanie" id="auth.login.title.main" />
        )}
      </Text>

      {passwordFlow ? (
        <AuthContextProvider isLoading={isLoadingPassword}>
          <LoginCard flow={passwordFlow} onSubmit={submitPassword} />
        </AuthContextProvider>
      ) : (
        <SkeletonLoader height={230} />
      )}

      {!is2FA && !isRefresh && (
        <Button asChild block type="tertiary">
          <Link to="/recovery">
            <FormattedMessage defaultMessage="Nie pamiętasz hasła?" id="auth.login.action.forgotPassword" />
          </Link>
        </Button>
      )}

      {needsSeparator && (
        <Separator>
          <FormattedMessage defaultMessage="lub zaloguj się przy użyciu" id="auth.login.label.orLoginWith" />
        </Separator>
      )}

      <OidcProviders>
        {facebookFlow ? (
          <AuthContextProvider isLoading={isLoadingFacebook}>
            <LoginCard flow={facebookFlow} onSubmit={submitFacebook} />
          </AuthContextProvider>
        ) : (
          <SkeletonLoader height={40} />
        )}

        {appleFlow ? (
          <AuthContextProvider isLoading={isLoadingApple}>
            <LoginCard flow={appleFlow} onSubmit={submitApple} />
          </AuthContextProvider>
        ) : (
          <SkeletonLoader height={40} />
        )}

        {googleFlow ? (
          <AuthContextProvider isLoading={isLoadingGoogle}>
            <LoginCard flow={googleFlow} onSubmit={submitGoogle} />
          </AuthContextProvider>
        ) : (
          <SkeletonLoader height={40} />
        )}

        {passkeyFlow ? (
          <AuthContextProvider isLoading={isLoadingPasskey}>
            <LoginCard flow={passkeyFlow} onSubmit={submitPasskey} />
          </AuthContextProvider>
        ) : (
          <SkeletonLoader height={40} />
        )}
      </OidcProviders>

      {!isRefresh && (
        <>
          <Separator />

          {is2FA ? (
            <Button block type="tertiary" onClick={logout}>
              <FormattedMessage defaultMessage="Wyloguj się" id="auth.login.action.logout" />
            </Button>
          ) : (
            <Button asChild block type="tertiary">
              <Link to="/registration">
                <FormattedMessage defaultMessage="Nie masz konta? Zarejestruj się" id="auth.login.action.register" />
              </Link>
            </Button>
          )}
        </>
      )}
    </AuthContainer>
  )
}

function useHandleLogin(returnTo?: string) {
  const nav = useNavigate()

  return useCallback(async () => {
    const session = (await kratosClient.toSession()).data
    sessionManager.setSession(session)

    if (returnTo) {
      nav({ to: returnTo })
      return
    }
    nav({ to: "/" })
  }, [nav, returnTo])
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
    node => node.group === "lookup_secret" || node.group === "password" || node.group === "default",
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
