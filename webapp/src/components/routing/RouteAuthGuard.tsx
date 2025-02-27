import { UserType } from "../../types";
import { useUserContext } from "../../providers";
import { Navigate, useLocation } from "react-router-dom";

type Props = {
    component: React.ReactNode;
    redirect: string;
    allowtypes?: UserType[];
};

export const RouteAuthGuard: React.FC<Props> = (props) => {
    const location = useLocation();
    const { user, isFetching } = useUserContext();

    if (isFetching) {
        return <></>;
    }
    
    const allowRoute = user ? (props.allowtypes ? props.allowtypes.includes(user.userType) : true) : false;

    if (!allowRoute) {
        return <Navigate to={props.redirect} state={{ from: location }} replace={true} />;
    }

    return <>{props.component}</>;
};
