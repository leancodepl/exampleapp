import { useEffect } from "react"
import { concatMap, filter, Observable, of, takeUntil, timer } from "rxjs"
import { SessionManager, sessionManager } from "../sessionManager"

export function useWaitForState(
  flow: unknown,
  selector: (sessionManager: SessionManager) => Observable<boolean | undefined>,
) {
  useEffect(() => {
    const subscription = of(100, 100, 100, 200, 200, 300, 500, 500, 500, 1000, 1000, 1000, 1000, 1000, 1000)
      .pipe(
        concatMap(delay => timer(delay)),
        takeUntil(selector(sessionManager).pipe(filter(Boolean))),
      )
      .subscribe(() => sessionManager.checkIfLoggedIn())

    return () => subscription.unsubscribe()
  }, [flow, selector])
}
