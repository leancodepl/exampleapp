import { Link } from "@tanstack/react-router"
import { Button, Flex, Media, Sidebar } from "@web/design-system"
import { IconUser01 } from "@web/design-system/icons"
import { TitlePlaceholder } from "../AppTitle"
import { AppTopbarContainer, AppTopbarMobileHeader, AppTopbarMobileSidebarTrigger, profileIconCss } from "./styles"

export function AppTopbar() {
  return (
    <AppTopbarContainer>
      <Media to="sm">
        <AppTopbarMobileHeader headline-medium color="default.primary">
          <TitlePlaceholder />
        </AppTopbarMobileHeader>

        <AppTopbarMobileSidebarTrigger>
          <Sidebar.Trigger />
        </AppTopbarMobileSidebarTrigger>
      </Media>

      <Media from="sm">
        <div />

        <Flex align="center" gap="large">
          <Button asChild leading={<IconUser01 className={profileIconCss} />} type="text">
            <Link to="/profile" />
          </Button>
        </Flex>
      </Media>
    </AppTopbarContainer>
  )
}
