export const formatDate = (dateString:string|null|undefined):string=>{
    if(!dateString)
        return 'Неизвестно';
    
    try{
        const cleanDateString = dateString.trim();
        const date = new Date(cleanDateString);
        
        if(isNaN(date.getTime())){
            console.warn('Invalid date string:', dateString);
            return 'Неизвестно';
        }
        
        return date.toLocaleDateString('ru-RU',{
            year:'numeric',
            month:'2-digit',
            day:'2-digit'
        });
    }catch (error) {
        console.error('Date formating error', error, dateString);
        return 'Неизвестно';
    }
}