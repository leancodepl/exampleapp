import { styled } from "@pigment-css/react"
import { colors, spacing } from "../../../utils"

export const DrawerItemContainer = styled.button`
  display: flex;
  gap: ${spacing._4};
  align-items: center;
  height: 72px;
  padding: ${spacing._2} ${spacing._4};

  color: ${colors.foreground.default.primary};
  font-weight: 400;
  font-size: 14px;
  line-height: 24px;
  letter-spacing: -0.01px;
  text-decoration: none;

  background: none;

  &:hover {
    background: ${colors.background.default.primary_hover};
  }

  &:active {
    background: ${colors.background.accent.tertiary};
  }

  &:not(:last-child) {
    border-bottom: 1px solid ${colors.foreground.default.tertiary};
  }
`

export const DrawerIcon = styled.div`
  display: contents;

  color: ${colors.foreground.default.secondary};
`
