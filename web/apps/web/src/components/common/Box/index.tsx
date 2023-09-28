import { isString } from "lodash";
import styled from "styled-components";
import { Spacing, spacings } from "../../../_styling/globals";

type Direction = "row" | "column" | "row-reverse" | "column-reverse";
type Alignment = "baseline" | "center" | "end" | "start" | "stretch";
type Justify = "flex-start" | "flex-end" | "center" | "space-between" | "space-around" | "space-evenly";
type Padding = Spacing | { x?: Spacing; y?: Spacing; top?: Spacing; right?: Spacing; bottom?: Spacing; left?: Spacing };
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
    gap: ${({ gap }) => gap && spacings[gap]};
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

    if (isString(padding)) {
        return spacings[padding];
    } else {
        top = padding.top ?? padding.y;
        right = padding.right ?? padding.x;
        bottom = padding.bottom ?? padding.y;
        left = padding.left ?? padding.x;
    }

    const paddings = [
        (top && spacings[top]) || 0,
        (right && spacings[right]) || 0,
        (bottom && spacings[bottom]) || 0,
        (left && spacings[left]) || 0,
    ];

    return paddings.join(" ");
};
