import { styled } from "@pigment-css/react"
import { colors, spacing } from "../../../utils"
import { dataSide, dataState } from "../attributes"

export const SidebarAnimation = styled("div", { name: "Sidebar", slot: "animation" })`
  position: relative;

  width: var(--sidebar-width);
  height: 100svh;

  background-color: transparent;

  transition: width 0.2s linear;

  ${dataState.variant("collapsed")} & {
    width: calc(var(--sidebar-width-icon) + ${spacing._4} * 4 + 1px);
  }
`

export const StyledSidebarRoot = styled("div", { name: "Sidebar", slot: "root" })`
  --sidebar-width: 272px;
  --sidebar-width-icon: 24px;
`

export const SidebarContainer = styled("div", { name: "Sidebar", slot: "container" })`
  position: fixed;
  top: 0;
  bottom: 0;
  z-index: 10;

  width: var(--sidebar-width);
  height: 100svh;

  transition:
    left 0.2s linear,
    right 0.2s linear,
    width 0.2s linear;

  ${dataSide.variant("left")} & {
    border-right: 1px solid ${colors.foreground.default.tertiary};
  }

  ${dataState.variant("collapsed")} & {
    width: calc(var(--sidebar-width-icon) + ${spacing._4} * 4 + 1px);
  }
`

export const SidebarContent = styled("div", { name: "Sidebar", slot: "content" })`
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  width: 100%;
  height: 100%;
`
