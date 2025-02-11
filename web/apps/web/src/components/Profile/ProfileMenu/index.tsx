import { FormattedMessage } from "react-intl"
import { Link, useNavigate } from "@tanstack/react-router"
import { Drawer, useIsMobile } from "@web/design-system"
import { IconFlag01, IconLock01, IconLogIn01 } from "@web/design-system/icons"
import { Title } from "../../AppTitle"
import { ProfileInfo } from "../ProfileInfo"

export function ProfileMenu() {
  const isMobile = useIsMobile()
  const nav = useNavigate()

  if (!isMobile) {
    nav({ to: "/profile/language", replace: true })
    return null
  }

  return (
    <>
      <ProfileInfo />

      <Title>
        <FormattedMessage defaultMessage="Profil i ustawienia" id="profile.menu.title.settings" />
      </Title>

      <Drawer.Root>
        <Drawer.Item asChild icon={<IconLogIn01 />}>
          <Link to="/profile/socials">
            <FormattedMessage defaultMessage="Metody logowania" id="profile.menu.label.loginMethods" />
          </Link>
        </Drawer.Item>
        <Drawer.Item asChild icon={<IconLock01 />}>
          <Link to="/profile/password">
            <FormattedMessage defaultMessage="Zmiana hasła" id="profile.menu.label.changePassword" />
          </Link>
        </Drawer.Item>
        <Drawer.Item asChild icon={<IconFlag01 />}>
          <Link to="/profile/language">
            <FormattedMessage defaultMessage="Język i wygląd" id="profile.menu.label.languageAndAppearance" />
          </Link>
        </Drawer.Item>
      </Drawer.Root>
    </>
  )
}
