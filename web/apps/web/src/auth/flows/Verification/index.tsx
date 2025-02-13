import { FormattedMessage } from "react-intl"
import { useNavigate } from "@tanstack/react-router"
import { SkeletonLoader, Text } from "@web/design-system"
import { useVerificationFlow, VerificationCard } from "@leancodepl/kratos"
import { useBoundRunInTask } from "@leancodepl/utils"
import { kratosClient } from "../.."
import { AuthContainer } from "../../components/AuthContainer"
import { AuthContextProvider } from "../../utils/authContext"

type VerificationFlowSearchParams = Exclude<Parameters<typeof useVerificationFlow>[0]["searchParams"], undefined>

type VerificationProps = {
  searchParams: VerificationFlowSearchParams
}

export function Verification({ searchParams }: VerificationProps) {
  const nav = useNavigate()

  const { flow, submit } = useVerificationFlow({
    kratosClient,
    onVerified: () => nav({ to: "/" }),
    updateSearchParams: search => nav({ to: "/verification", search }),
    searchParams,
  })

  const [isLoadingVerification, submitVerification] = useBoundRunInTask(submit)

  return (
    <AuthContainer>
      <Text headline-large color="default.primary">
        <FormattedMessage defaultMessage="Weryfikacja" id="auth.verification.title.main" />
      </Text>

      {flow ? (
        <AuthContextProvider isLoading={isLoadingVerification}>
          <VerificationCard flow={flow} onSubmit={submitVerification} />
        </AuthContextProvider>
      ) : (
        <SkeletonLoader height={230} />
      )}
    </AuthContainer>
  )
}
