import { styled } from "@pigment-css/react"
import { Slot } from "@radix-ui/react-slot"
import { breakpoints, colors, radii, spacing } from "../../../utils"
import { dataActive } from "../attributes"

export const SidebarMenuButtonSlot = styled(Slot, { name: "Sidebar", slot: "menuButton" })`
  display: flex;
  gap: ${spacing._4};
  align-items: center;
  width: 100%;
  height: 48px;
  overflow: hidden;

  color: ${colors.foreground.accent.primary};
  white-space: nowrap;
  text-decoration: none;

  background: none;
  border: none;
  border-radius: ${radii.lg};
  outline: none;
  cursor: pointer;
  padding-inline: ${spacing._4};
  padding-block: ${spacing._3};

  &:hover,
  &:focus {
    background: ${colors.background.default.primary_hover};
  }

  &:focus-visible {
    outline: 1px solid black;
  }

  &:active,
  ${dataActive.variant("")}& {
    background: ${colors.background.accent.tertiary};
  }

  ${breakpoints.down.sm} {
    height: 56px;
  }
`
