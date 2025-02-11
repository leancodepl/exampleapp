import { useCallback } from "react"
import { FormattedMessage } from "react-intl"
import { Link, useNavigate } from "@tanstack/react-router"
import { Media, Separator, Sidebar, Text, useIsMobile } from "@web/design-system"
import { IconList, IconLogOut01, IconUser01 } from "@web/design-system/icons"
import { useLogoutFlow } from "@leancodepl/kratos"
import { kratosClient } from "../../auth"
import { sessionManager } from "../../auth/sessionManager"
import { SidebarIcon, SidebarLogo } from "./styles"

export function AppSidebar() {
  const nav = useNavigate()

  const { logout } = useLogoutFlow({
    kratosClient,
    onLoggedOut: useCallback(() => {
      nav({ to: "/login" })
      sessionManager.checkIfLoggedIn()
    }, [nav]),
  })

  const { toggleSidebar } = Sidebar.useSidebar()
  const isMobile = useIsMobile()

  const toggleSidebarOnMobile = useCallback(() => {
    isMobile && toggleSidebar()
  }, [isMobile, toggleSidebar])

  return (
    <Sidebar.Root>
      <Sidebar.Header>
        <Link to="/" onClick={toggleSidebarOnMobile}>
          <SidebarLogo />
        </Link>
        <Sidebar.Trigger />
      </Sidebar.Header>

      <Sidebar.Content>
        <Sidebar.Group>
          <Sidebar.GroupContent>
            <Sidebar.Menu>
              <Sidebar.MenuItem>
                <Sidebar.MenuButton
                  tooltip={<FormattedMessage defaultMessage="Harmonogram" id="sidebar.menu.payments.tooltip" />}>
                  <Link activeProps={activeProps} to="/" onClick={toggleSidebarOnMobile}>
                    <SidebarIcon>
                      <IconList />
                    </SidebarIcon>
                    <Text body>
                      <FormattedMessage defaultMessage="Harmonogram" id="sidebar.menu.payments.label" />
                    </Text>
                  </Link>
                </Sidebar.MenuButton>
              </Sidebar.MenuItem>
            </Sidebar.Menu>
          </Sidebar.GroupContent>
        </Sidebar.Group>

        <Sidebar.Group>
          <Media to="sm">
            <Separator />
          </Media>

          <Sidebar.GroupContent>
            <Sidebar.Menu>
              <Media to="sm">
                <Sidebar.MenuItem>
                  <Sidebar.MenuButton
                    tooltip={
                      <FormattedMessage defaultMessage="Profil i ustawienia" id="sidebar.menu.profile.tooltip" />
                    }>
                    <Link activeProps={activeProps} to="/profile" onClick={toggleSidebarOnMobile}>
                      <SidebarIcon>
                        <IconUser01 />
                      </SidebarIcon>
                      <Text body>
                        <FormattedMessage defaultMessage="Profil i ustawienia" id="sidebar.menu.profile.label" />
                      </Text>
                    </Link>
                  </Sidebar.MenuButton>
                </Sidebar.MenuItem>
              </Media>

              <Sidebar.MenuItem>
                <Sidebar.MenuButton
                  tooltip={<FormattedMessage defaultMessage="Wyloguj" id="sidebar.menu.logout.tooltip" />}>
                  <button onClick={logout}>
                    <SidebarIcon>
                      <IconLogOut01 />
                    </SidebarIcon>
                    <Text body>
                      <FormattedMessage defaultMessage="Wyloguj" id="sidebar.menu.logout.label" />
                    </Text>
                  </button>
                </Sidebar.MenuButton>
              </Sidebar.MenuItem>
            </Sidebar.Menu>
          </Sidebar.GroupContent>
        </Sidebar.Group>
      </Sidebar.Content>
    </Sidebar.Root>
  )
}

const activeProps = { "data-active": "" }
