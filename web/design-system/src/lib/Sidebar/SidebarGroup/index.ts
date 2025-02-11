import { styled } from "@pigment-css/react"

export const SidebarGroup = styled("div", { name: "Sidebar", slot: "group" })(() => ({
  position: "relative",
  display: "flex",
  width: "100%",
  minWidth: 0,
  flexDirection: "column",
}))
