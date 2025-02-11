import { css, styled } from "@pigment-css/react"
import { breakpoints, colors, spacing, Text } from "@web/design-system"

export const AppTopbarContainer = styled.div`
  z-index: 2;

  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
  padding: ${spacing._1} ${spacing._6};

  background: ${colors.background.default.primary};
  border-bottom: 1px solid ${colors.foreground.default.tertiary};

  ${breakpoints.down.md} {
    position: sticky;
    top: 0;

    min-height: 73px;
  }

  ${breakpoints.up.md} {
    height: 73px;
  }
`

export const AppTopbarMobileEstatesContainer = styled.div`
  padding-inline: ${spacing._6};

  padding-top: ${spacing._6};
`

export const AppTopbarMobileHeader = styled(Text)`
  display: contents;
`

export const AppTopbarMobileSidebarTrigger = styled.div`
  height: 100%;
  padding-top: ${spacing._4};
`

export const profileIconCss = css`
  color: ${colors.foreground.default.secondary};
`
