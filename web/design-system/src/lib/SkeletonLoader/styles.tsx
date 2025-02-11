import { styled } from "@pigment-css/react"
import { radii } from "../.."

export const SkeletonLoaderRoot = styled.div`
  flex: 1;
  min-height: var(--skeleton-height, 100px);

  border-radius: ${radii.md};
`
