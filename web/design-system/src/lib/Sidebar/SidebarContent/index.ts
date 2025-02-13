import { styled } from "@pigment-css/react"
import { spacing } from "../../.."

export const SidebarContent = styled("div", { name: "Sidebar", slot: "content" })`
  display: flex;
  flex: 1;
  flex-direction: column;
  gap: ${spacing._2};
  justify-content: space-between;
  min-height: 0;
  padding: ${spacing._4};
  overflow: auto;
`
