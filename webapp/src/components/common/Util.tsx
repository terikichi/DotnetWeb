export function stringToBoolean(value: string | null | undefined): boolean {
    if (!value) {
        return false;
    }
    return value.toLowerCase() === 'true' || value === '1';
}

