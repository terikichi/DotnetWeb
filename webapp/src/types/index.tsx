export const UserType = {
    Normal: '0',
    Premium: '1',
    SystemOperator: '2',
    SystemAdministrator: '9',
} as const;

export type UserType = keyof typeof UserType;
export const AllUserTypes: UserType[] = Object.keys(UserType) as UserType[];

export const UserState = {
    Active: '0',
    Suspend: '1',
    Banned: '9',
} as const;

export type UserState = keyof typeof UserState;
export const AllUserState: UserState[] = Object.keys(UserState) as UserState[];

export type User = {
    id: string;
    name: string;
    userType: UserType;
    userState: UserState;
}

export const numberToUserType = (typeNo: number): UserType => {
    switch (typeNo) {
        case 1:
            return "Normal";
        case 2:
            return "Premium";
        case 9:
            return "SystemOperator";
        case 0:
            return "SystemAdministrator";
        default:
            return "Normal";
    }
}

export const numberToUserState = (typeNo: number): UserState => {
    switch (typeNo) {
        case 0:
            return "Active";
        case 1:
            return "Suspend";
        case 9:
            return "Banned";
        default:
            return "Suspend";
    }
}