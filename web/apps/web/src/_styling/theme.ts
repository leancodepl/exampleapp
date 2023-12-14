import { px2rem } from "./px2rem";

export const theme = {
    color: {
        primary: "#26C971",
        pageBackground: "#F0F2F5",
    },
    spacing: {
        xsmall: px2rem(4),
        small: px2rem(8),
        medium: px2rem(16),
        large: px2rem(24),
    },
    fontSize: {
        xsmall: px2rem(12),
        small: px2rem(14),
        medium: px2rem(16),
        large: px2rem(18),
    },
    borderRadius: {
        default: "6px",
    },
} as const;

export type Theme = typeof theme;
export type Spacing = keyof Theme["spacing"];

declare module "styled-components" {
    // eslint-disable-next-line @typescript-eslint/no-empty-interface
    export interface DefaultTheme extends Theme {}
}
