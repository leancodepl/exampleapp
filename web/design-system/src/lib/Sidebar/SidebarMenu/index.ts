import { styled } from "@pigment-css/react"
import { spacing } from "../../.."

export const SidebarMenu = styled("ul", { name: "Sidebar", slot: "menu" })`
  display: flex;
  flex-direction: column;
  gap: ${spacing._1};
  width: 100%;
  min-width: 0;
`
