import { px2rem } from "./px2rem";

export const primaryColor = "#26C971";
export const pageBackground = "#F0F2F5";

export const spacingXSmall = px2rem(4);
export const spacingSmall = px2rem(8);
export const spacingMedium = px2rem(16);
export const spacingLarge = px2rem(24);

export const spacings = {
    xsmall: spacingXSmall,
    small: spacingSmall,
    medium: spacingMedium,
    large: spacingLarge,
} as const;

export type Spacing = keyof typeof spacings;

export const fontXSmall = px2rem(12);
export const fontSmall = px2rem(14);
export const fontMedium = px2rem(16);
export const fontLarge = px2rem(18);

export const defaultBorderRadius = "6px";
