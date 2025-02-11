import { FormattedMessage } from "react-intl"
import { Link } from "@tanstack/react-router"
import { Button, Flex, Text } from "@web/design-system"
import { useIsLoggedIn } from "../../auth/useIsLoggedIn"
import { Error404, ErrorContainer } from "./styles"

export function NotFound() {
  const isLoggedIn = useIsLoggedIn()

  const content = (
    <ErrorContainer>
      <Error404>404</Error404>
      <Flex align="center" direction="column" gap="small">
        <Text headline-medium color="default.primary">
          <FormattedMessage defaultMessage="Strona nie została znaleziona" id="notFound.title" />
        </Text>

        <Flex align="center" direction="column">
          <Text body color="default.primary">
            <FormattedMessage defaultMessage="Strona, której szukasz, nie istnieje." id="notFound.description.main" />
          </Text>
          <Text body color="default.primary">
            <FormattedMessage
              defaultMessage="Możliwe, że zmienił się jej adres lub została usunięta."
              id="notFound.description.details"
            />
          </Text>
        </Flex>
      </Flex>
      <Button asChild size="large">
        <Link to="/">
          <FormattedMessage defaultMessage="Wróć do strony głównej" id="notFound.action.backToHome" />
        </Link>
      </Button>
    </ErrorContainer>
  )

  if (isLoggedIn) {
    return <>{content}</>
  }

  return content
}
