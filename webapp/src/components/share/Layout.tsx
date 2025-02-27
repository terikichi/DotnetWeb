import { Outlet, Link } from "react-router-dom";
import { useUserContext } from "../../providers";
import { UserType, AllUserTypes } from "../../types";

type Page = {
    title: string;
    path: string;
    isAutorized?: boolean;
    IsAnonymous?: boolean;
    allowtypes?: UserType[];
}

const pages: Page[] = [
    { title: "Home", path: "/", IsAnonymous: true },
    { title: "PrivatePage", path: "/PrivatePage", isAutorized: true, allowtypes: AllUserTypes },
    { title: "ChangePassword", path: "/User/ChangePassword", isAutorized: true, allowtypes: AllUserTypes },
    { title: "Edit", path: "/User/Edit", isAutorized: true, allowtypes: AllUserTypes },
    { title: "Delete", path: "/User/Delete", isAutorized: true, allowtypes: ["SystemOperator", "SystemAdministrator"] }
];

export const Layout: React.FC = () => {
    const { user, isFetching } = useUserContext();

    const renderPages = () => {
        if (user) {
            return pages.filter(page => page.IsAnonymous == true || (page.isAutorized == true && page.allowtypes?.includes(user.userType)));
        } else {
            return pages.filter(page => page.IsAnonymous);
        }
    };

    if (isFetching) {
        return <></>;
    }

    return (
        <>
            <nav>
                <div className="naviItemContainer">
                    <h1 className="naviItem appTitle">React.NET</h1>
                    <div className="naviItem naviBarDivider"/>
                    {renderPages().map((page) => (
                        <Link key={page.path} className="naviItem naviMenuItem" to={page.path}>{page.title}</Link>
                    ))}
                    <div className="naviItem flexBlank"/>
                    {user ? <><div className="naviItem userName">{user?.name}</div><Link className="naviItem" to="/Logout"><button className="btnFrame color2">Logout</button></Link></>
                            : <><Link  className="naviItem" to="/Login"><button className="btnLink coler1">Login</button></Link><Link className="naviItem" to="/SignUp"><button className="btnFrame color2">Sign Up</button></Link></>}
                </div>
            </nav>

            <Outlet />
        </>
        
    )
}