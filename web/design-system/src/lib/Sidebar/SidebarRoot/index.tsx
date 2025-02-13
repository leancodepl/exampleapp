import { ComponentPropsWithoutRef, forwardRef } from "react"
import * as VisuallyHidden from "@radix-ui/react-visually-hidden"
import { Sheet, useIsMobile } from "../../.."
import { Media } from "../../Media"
import { dataSide, dataState } from "../attributes"
import { useSidebar } from "../SidebarProvider"
import { SidebarAnimation, SidebarContainer, SidebarContent, StyledSidebarRoot } from "./styles"

type SidebarRootProps = {
  side?: "left" | "right"
} & ComponentPropsWithoutRef<"div">

export const SidebarRoot = forwardRef<HTMLDivElement, SidebarRootProps>(
  ({ side = "left", children, ...props }, ref) => {
    const { state, openMobile } = useSidebar()
    const isMobile = useIsMobile()

    return (
      <StyledSidebarRoot ref={ref} {...dataState(state)} {...dataSide(side)}>
        <Media to="sm">
          <Sheet.Root open={isMobile && openMobile}>
            <VisuallyHidden.Root>
              <Sheet.Title>LeanBooking</Sheet.Title>
            </VisuallyHidden.Root>

            <Sheet.Content>
              <SidebarContent data-sidebar="sidebar">{children}</SidebarContent>
            </Sheet.Content>
          </Sheet.Root>
        </Media>

        <Media from="sm">
          <SidebarAnimation />

          <SidebarContainer {...props}>
            <SidebarContent data-sidebar="sidebar">{children}</SidebarContent>
          </SidebarContainer>
        </Media>
      </StyledSidebarRoot>
    )
  },
)
