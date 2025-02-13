import { useMemo } from "react"
import { FormattedMessage } from "react-intl"
import { Link, Outlet, useMatches } from "@tanstack/react-router"
import { Flex, Media, TabBar, Text, useIsMobile } from "@web/design-system"
import { Route as LanguageRoute } from "../../routes/_authorized/profile/language"
import { Route as PasswordRoute } from "../../routes/_authorized/profile/password"
import { Route as SocialsRoute } from "../../routes/_authorized/profile/socials"
import { Title, TitlePlaceholder } from "../AppTitle"
import { ProfileInfo } from "./ProfileInfo"
import { ProfileTabsSeparator } from "./style"

export function Profile() {
  const matches = useMatches()
  const isMobile = useIsMobile()

  const currentRoute = useMemo(() => {
    for (const match of matches.toReversed()) {
      if (match.id === SocialsRoute.id) return "socials"
      if (match.id === PasswordRoute.id) return "password"
      if (match.id === LanguageRoute.id) return "language"
    }
  }, [matches])

  return (
    <Flex direction="column" gap="medium">
      {!isMobile && (
        <Title>
          <FormattedMessage defaultMessage="Profil i ustawienia" id="profile.title.settings" />
        </Title>
      )}

      <Media from="sm">
        <Flex direction="column" gap="large">
          <Text headline-medium color="default.primary">
            <TitlePlaceholder />
          </Text>

          <ProfileInfo />
        </Flex>

        <div>
          <TabBar.Root value={currentRoute}>
            <TabBar.Tabs>
              <TabBar.Tab asChild value={routes.language}>
                <Link to="/profile/language">
                  <FormattedMessage defaultMessage="Język i wygląd" id="profile.tab.label.language" />
                </Link>
              </TabBar.Tab>

              <TabBar.Tab asChild value={routes.socials}>
                <Link to="/profile/socials">
                  <FormattedMessage defaultMessage="Metody logowania" id="profile.tab.label.login" />
                </Link>
              </TabBar.Tab>

              <TabBar.Tab asChild value={routes.password}>
                <Link to="/profile/password">
                  <FormattedMessage defaultMessage="Zmiana hasła" id="profile.tab.label.password" />
                </Link>
              </TabBar.Tab>
            </TabBar.Tabs>
          </TabBar.Root>
          <ProfileTabsSeparator />
        </div>
      </Media>

      <Outlet />
    </Flex>
  )
}

const routes = {
  socials: "socials",
  password: "password",
  language: "language",
}
