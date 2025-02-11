import { useObservable } from "react-use"
import { sessionManager } from "./sessionManager"

export function useIsLoggedIn() {
  return useObservable(sessionManager.isLoggedIn)
}
