import React, { ReactNode } from "react"
import { Tooltip } from "../../.."
import { Content, Root } from "../../Tooltip"
import { useSidebar } from "../SidebarProvider"
import { SidebarMenuButtonSlot } from "./styles"

type SidebarMenuButtonProps = {
  children?: ReactNode
  tooltip?: ReactNode
}

export function SidebarMenuButton({ tooltip, children }: SidebarMenuButtonProps) {
  const { isMobile, state } = useSidebar()

  const button = <SidebarMenuButtonSlot>{children}</SidebarMenuButtonSlot>

  if (!tooltip || state !== "collapsed") return button

  return (
    <Root>
      <Tooltip.Trigger>{button}</Tooltip.Trigger>

      <Content inverse align="center" hidden={isMobile} side="right">
        {tooltip}
      </Content>
    </Root>
  )
}
