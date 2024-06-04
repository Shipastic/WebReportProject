export const getSortedData = (data: any[], sortColumn: string | null, sortType: 'asc' | 'desc' | undefined) => {
    if (sortColumn && sortType) {
        return [...data].sort((a, b) => {
            const x = a[sortColumn];
            const y = b[sortColumn];

            if (typeof x === 'number' && typeof y === 'number') {
                return sortType === 'asc' ? x - y : y - x;
            }

            if (typeof x === 'string' && typeof y === 'string') {
                return sortType === 'asc' ? x.localeCompare(y) : y.localeCompare(x);
            }

            console.warn('Unsupported sort type or values', x, y);
            return 0;
        });
    }
    return data;
};

export default getSortedData;