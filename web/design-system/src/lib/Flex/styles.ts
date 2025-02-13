import { css, styled } from "@pigment-css/react"
import { spacing } from "../../utils"

export const gapMap = {
  xsmall: css`
    gap: ${spacing._1};
  `,
  small: css`
    gap: ${spacing._2};
  `,
  medium: css`
    gap: ${spacing._4};
  `,
  large: css`
    gap: ${spacing._6};
  `,
}

export const directionMap = {
  column: css`
    flex-direction: column;
  `,
  row: css`
    flex-direction: row;
  `,
}

export const alignMap = {
  baseline: css`
    align-items: baseline;
  `,
  center: css`
    align-items: center;
  `,
}

export const justifyMap = {
  center: css`
    justify-content: center;
  `,
  "flex-start": css`
    justify-content: flex-start;
  `,
  "space-between": css`
    justify-content: space-between;
  `,
}

export const paddingMap = {
  small: css`
    padding: ${spacing._2};
  `,
  medium: css`
    padding: ${spacing._4};
  `,
  large: css`
    padding: ${spacing._6};
  `,
}

export const paddingTopMap = {
  small: css`
    padding-top: ${spacing._2};
  `,
  medium: css`
    padding-top: ${spacing._4};
  `,
  large: css`
    padding-top: ${spacing._6};
  `,
}

export const paddingLeftMap = {
  small: css`
    padding-left: ${spacing._2};
  `,
  medium: css`
    padding-left: ${spacing._4};
  `,
  large: css`
    padding-left: ${spacing._6};
  `,
}

export const paddingRightMap = {
  small: css`
    padding-right: ${spacing._2};
  `,
  medium: css`
    padding-right: ${spacing._4};
  `,
  large: css`
    padding-right: ${spacing._6};
  `,
}

export const paddingBottomMap = {
  small: css`
    padding-bottom: ${spacing._2};
  `,
  medium: css`
    padding-bottom: ${spacing._4};
  `,
  large: css`
    padding-bottom: ${spacing._6};
  `,
}

export const FlexRoot = styled.div`
  display: flex;
`
