import { Suspense, lazy } from "react";
import { Route, Routes } from "react-router";
import { LoggedInGuard } from "../components/auth/LoggedInGuard";
import { NotLoggedInGuard } from "../components/auth/NotLoggedInGuard";
import { SplashScreen } from "../components/common/SplashScreen";
import { UnauthenticatedLayout } from "../components/common/UnauthenticatedLayout";
import { LoginPage } from "../pages/login";
import { RecoveryPage } from "../pages/recovery";
import { RegisterPage } from "../pages/register";
import { VerificationPage } from "../pages/verification";
import { loginRoute, recoveryRoute, registerRoute, verificationRoute } from "../publicRoutes";

export function App() {
    return (
        <Routes>
            <Route>
                <Route element={<UnauthenticatedLayout />}>
                    <Route element={<LoginPage />} path={loginRoute} />
                    <Route element={<NotLoggedInGuard />}>
                        <Route element={<RegisterPage />} path={registerRoute} />
                        <Route element={<VerificationPage />} path={verificationRoute} />
                        <Route element={<RecoveryPage />} path={recoveryRoute} />
                    </Route>
                </Route>
            </Route>

            <Route element={<LoggedInGuard />}>
                <Route
                    element={
                        <Suspense fallback={<SplashScreen />}>
                            <AuthenticatedApp />
                        </Suspense>
                    }
                    path="*"
                />
            </Route>
        </Routes>
    );
}

const AuthenticatedApp = lazy(() => import("./authenticatedApp").then(m => ({ default: m.AuthenticatedApp })));
