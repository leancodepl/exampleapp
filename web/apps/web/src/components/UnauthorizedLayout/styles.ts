import { css, styled } from "@pigment-css/react"
import { spacing } from "@web/design-system"

export const Root = styled.div`
  display: flex;
  max-width: 200vh;
  margin: 0 auto;
  flex-direction: column;
  min-height: 100dvh;
  padding: ${spacing._6};
`

export const Content = styled.div`
  display: flex;
  flex: 1;
  flex-direction: column;
  gap: ${spacing._12};
  align-items: center;
  align-self: center;
  justify-content: center;
  width: 100%;
  max-width: 432px;
`

export const languageSelectCss = css`
  width: 120px;
  min-width: unset;
`
