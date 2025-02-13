import { styled } from "@pigment-css/react"
import { colors, radii, spacing } from "@web/design-system"

export const ProfileListItem = styled.div`
  padding: ${spacing._4};

  border: 1px solid ${colors.foreground.default.tertiary};

  &:first-of-type {
    border-top-left-radius: ${radii.md};
    border-top-right-radius: ${radii.md};
  }

  &:not(:first-child) {
    margin-top: -1px;
  }

  &:last-of-type {
    border-bottom-right-radius: ${radii.md};
    border-bottom-left-radius: ${radii.md};
  }

  &[data-state="checked"] {
    z-index: 1;

    border-color: ${colors.foreground.accent.primary};
  }
`

export const ProfileContainerContent = styled.div`
  max-width: 432px;
`

export const ProfileListItems = styled.div`
  display: flex;
  flex-direction: column;
`
