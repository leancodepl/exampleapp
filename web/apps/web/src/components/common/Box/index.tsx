import { Spacing } from "_styling/theme";
import styled, { css } from "styled-components";

type Direction = "row" | "column" | "row-reverse" | "column-reverse";
type Alignment = "baseline" | "center" | "end" | "start" | "stretch";
type Justify = "flex-start" | "flex-end" | "center" | "space-between" | "space-around" | "space-evenly";
type Padding =
    | Spacing
    | {
          x?: Spacing;
          y?: Spacing;
          top?: Spacing;
          right?: Spacing;
          bottom?: Spacing;
          left?: Spacing;
      };
type Wrap = "wrap" | "wrap-reverse";

export type BoxProps = {
    justify?: Justify;
    align?: Alignment;
    gap?: Spacing;
    direction?: Direction;
    padding?: Padding;
    wrap?: Wrap;
};

export const Box = styled.div<BoxProps>`
    display: flex;
    flex-direction: ${({ direction }) => direction};
    flex-wrap: ${({ wrap }) => wrap};
    gap: ${({ gap, theme }) => gap && theme.spacing[gap]};
    align-items: ${({ align }) => align};
    justify-content: ${({ justify }) => justify};
    padding: ${({ padding }) => padding && cssPadding(padding)};
`;

const cssPadding = (padding?: Padding) => {
    if (!padding) {
        return null;
    }

    let top: Spacing | undefined = undefined;
    let right: Spacing | undefined = undefined;
    let bottom: Spacing | undefined = undefined;
    let left: Spacing | undefined = undefined;

    if (typeof padding === "string") {
        return css`
            padding: ${({ theme }) => theme.spacing[padding]};
        `;
    } else {
        top = padding.top ?? padding.y;
        right = padding.right ?? padding.x;
        bottom = padding.bottom ?? padding.y;
        left = padding.left ?? padding.x;
    }

    const paddings = [top || 0, right || 0, bottom || 0, left || 0];

    return css`
        padding: ${({ theme }) =>
            paddings.map(padding => (typeof padding === "number" ? padding : theme.spacing[padding])).join(" ")};
    `;
};
