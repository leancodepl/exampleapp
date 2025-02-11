import { useCallback, useEffect, useMemo, useState } from "react"
import { FormattedMessage } from "react-intl"
import { ContinueWith } from "@ory/client"
import { Link, useNavigate } from "@tanstack/react-router"
import { Button, Flex, SkeletonLoader, Text } from "@web/design-system"
import { IconArrowLeft } from "@web/design-system/icons"
import {
  InfoNodeLabel,
  KratosComponents,
  KratosContextProvider,
  RecoveryCard,
  useRecoveryFlow,
  UserSettingsCard,
} from "@leancodepl/kratos"
import { useBoundRunInTask } from "@leancodepl/utils"
import { kratosClient } from "../.."
import { AuthContainer } from "../../components/AuthContainer"
import { MessageFormat } from "../../components/MessageFormat"
import { useSettingsFlow } from "../../hooks/useSettingsFlow"
import { sessionManager } from "../../sessionManager"
import { AuthContextProvider } from "../../utils/authContext"

export type RecoverySearchParams = Parameters<typeof useRecoveryFlow>[0]["searchParams"]

type RecoveryProps = {
  searchParams: RecoverySearchParams
}

export function Recovery({ searchParams = {} }: RecoveryProps) {
  const [settingsFlowId, setSettingsFlowId] = useState<string>()

  if (settingsFlowId) {
    return <UserSettingsFlow settingsFlowId={settingsFlowId} />
  }
  return <RecoveryFlow searchParams={searchParams} onShowSettingsUi={setSettingsFlowId} />
}

type RecoveryFlowProps = {
  searchParams: RecoverySearchParams
  onShowSettingsUi: (settingsFlowId: string) => void
}

function RecoveryFlow({ searchParams, onShowSettingsUi }: RecoveryFlowProps) {
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
            onShowSettingsUi(continuation.flow.id)
            break
          case "show_verification_ui":
            break
        }
      }
    },
    [nav, onShowSettingsUi],
  )

  const { flow, submit } = useRecoveryFlow({
    kratosClient,
    onSessionAlreadyAvailable: () => sessionManager.checkIfLoggedIn(),
    onContinueWith,
    searchParams,
    updateSearchParams: search => nav({ to: "/recovery", search }),
  })

  const [isLoadingRecovery, submitRecovery] = useBoundRunInTask(submit)

  const recoveryComponents = useMemo<Partial<KratosComponents>>(() => {
    return {
      MessageFormat: ({ id, text, context }) => {
        switch (id) {
          case InfoNodeLabel.InfoNodeLabelSubmit:
            if (flow?.state === "choose_method" && flow.active === "link") {
              return <FormattedMessage defaultMessage="Wyślij link ponownie" id="recovery.action.resendLink" />
            }

            if (flow?.state === "sent_email" && flow.active === "code") {
              return <FormattedMessage defaultMessage="Potwierdź kod" id="recovery.action.confirmCode" />
            }

            if (flow?.state === "sent_email" && flow.active === "link") {
              return <FormattedMessage defaultMessage="Wyślij ponownie" id="recovery.action.resend" />
            }
            return <FormattedMessage defaultMessage="Wyślij kod odzyskiwania" id="recovery.action.sendRecoveryCode" />
        }

        return <MessageFormat context={context} id={id} text={text} />
      },
    }
  }, [flow?.active, flow?.state])

  return (
    <AuthContainer>
      <Button asChild leading={<IconArrowLeft />} type="text">
        <Link to="/login">
          <FormattedMessage defaultMessage="Wróć do strony logowania" id="recovery.action.backToLogin" />
        </Link>
      </Button>

      <Flex direction="column" gap="small">
        <div>
          <Text headline-large color="default.primary">
            {(() => {
              if (flow?.state === "sent_email" && flow.active === "code") {
                return <FormattedMessage defaultMessage="Podaj kod" id="recovery.title.enterCode" />
              }

              if (flow?.state === "sent_email" && flow.active === "link") {
                return <FormattedMessage defaultMessage="Wyślij link ponownie" id="recovery.title.resendLink" />
              }

              return <FormattedMessage defaultMessage="Odzyskaj dostęp do konta" id="recovery.title.recoverAccess" />
            })()}
          </Text>
        </div>

        <div>
          <Text body color="default.primary">
            {(() => {
              if (flow?.state === "choose_method" && flow.active === "link") {
                return (
                  <FormattedMessage
                    defaultMessage={`Kliknij "Wyślij link ponownie", aby otrzymać link do odzyskania dostępu do Twojego konta`}
                    id="recovery.description.clickResendLink"
                  />
                )
              }

              if (flow?.state === "sent_email" && flow.active === "code") {
                return (
                  <FormattedMessage
                    defaultMessage="Sprawdź maila i podaj kod odzyskiwania poniżej, żeby odzyskać dostęp do swojego konta"
                    id="recovery.description.checkEmailForCode"
                  />
                )
              }

              if (flow?.state === "sent_email" && flow.active === "link") {
                return (
                  <FormattedMessage
                    defaultMessage="Sprawdź maila i użyj linku do odzyskiwania dostępu, Jeśli mail nie przyjdzie w ciągu 5 minut, wyślij link ponownie"
                    id="recovery.description.checkEmailForLink"
                  />
                )
              }

              return (
                <FormattedMessage
                  defaultMessage={`Kliknij "Wyślij kod odzyskiwania" aby otrzymać kod do odzyskania dostępu do Twojego konta`}
                  id="recovery.description.clickSendCode"
                />
              )
            })()}
          </Text>
        </div>
      </Flex>

      {flow ? (
        <KratosContextProvider components={recoveryComponents}>
          <AuthContextProvider isLoading={isLoadingRecovery}>
            <RecoveryCard flow={flow} onSubmit={submitRecovery} />
          </AuthContextProvider>
        </KratosContextProvider>
      ) : (
        <SkeletonLoader height={135} />
      )}
    </AuthContainer>
  )
}

type SettingsFlowProps = {
  settingsFlowId: string
}

function UserSettingsFlow({ settingsFlowId }: SettingsFlowProps) {
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
            break
        }
      }
    },
    [nav],
  )

  const { flow, submit } = useSettingsFlow({
    kratosClient,
    onContinueWith,
    settingsFlowId,
  })

  useEffect(() => {
    if (!flow) return

    if (flow.state === "success") {
      sessionManager.checkIfLoggedIn()
      nav({ to: "/" })
    }
  }, [flow, nav])

  const signInWithoutSettingPassword = useCallback(() => {
    sessionManager.checkIfLoggedIn()
    nav({ to: "/" })
  }, [nav])

  const [isLoadingSettings, submitSettings] = useBoundRunInTask(submit)

  return (
    <AuthContainer>
      <Flex direction="column" gap="small">
        <div>
          <Text headline-large color="default.primary">
            <FormattedMessage defaultMessage="Ustaw nowe hasło" id="recovery.settings.title.setNewPassword" />
          </Text>
        </div>

        <div>
          <Text color="default.primary">
            <FormattedMessage
              defaultMessage="Ustaw nowe hasło lub zaloguj się bezpośrednio do panelu"
              id="recovery.settings.description.setPasswordOrLogin"
            />
          </Text>
        </div>
      </Flex>

      {flow ? (
        <AuthContextProvider isLoading={isLoadingSettings}>
          <UserSettingsCard flow={flow} flowType="password" onSubmit={submitSettings} />
        </AuthContextProvider>
      ) : (
        <SkeletonLoader height={199} />
      )}

      <Button block type="tertiary" onClick={signInWithoutSettingPassword}>
        <FormattedMessage
          defaultMessage="Zaloguj się bez ustawiania hasła"
          id="recovery.settings.action.loginWithoutPassword"
        />
      </Button>
    </AuthContainer>
  )
}
