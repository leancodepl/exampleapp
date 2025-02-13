import { styled } from "@pigment-css/react"
import { colors, spacing } from "../../../utils"
import { dataState } from "../attributes"

export const SidebarHeader = styled("div", { name: "Sidebar", slot: "header" })`
  display: flex;
  gap: ${spacing._4};
  align-items: center;
  justify-content: space-between;
  min-width: 0;
  padding: ${spacing._4};

  border-bottom: 1px solid ${colors.foreground.default.tertiary};

  transition:
    margin 0.2s linear,
    padding 0.2s linear;

  ${dataState.variant("collapsed")} & {
    margin-left: calc(var(--sidebar-width-icon) - var(--sidebar-width));
    padding-right: ${spacing._6};
  }
`
