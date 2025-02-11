import { FormattedMessage } from "react-intl"
import { Link } from "@tanstack/react-router"
import { Button, Flex, useIsMobile } from "@web/design-system"
import { IconChevronLeft } from "@web/design-system/icons"
import { PasswordSettingsFlow } from "../../../auth/flows/PasswordSettings"
import { Title } from "../../AppTitle"
import { ProfileContainer } from "../ProfileContainer"

export function ProfilePassword() {
  const isMobile = useIsMobile()

  return (
    <ProfileContainer>
      {isMobile && (
        <Title>
          <Flex align="center" gap="small">
            <Button asChild leading={<IconChevronLeft />} type="text">
              <Link to="/profile" />
            </Button>
            <FormattedMessage defaultMessage="Zmiana hasła" id="profile.password.title" />
          </Flex>
        </Title>
      )}
      <PasswordSettingsFlow />
    </ProfileContainer>
  )
}
